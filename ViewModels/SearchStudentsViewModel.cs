﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Media.Media3D;
using UniversityDBExplorer.Models;
using UniversityDBExplorer.Special;
using UniversityDBExplorer.Special.Mediator;
using UniversityDBExplorer.Views;

namespace UniversityDBExplorer.ViewModels;

public class SearchStudentsViewModel : INotifyPropertyChanged
{
    private ObservableCollection<StudentModel> searchedStudents;
    private string searchSearchStudents;
    private RelayCommand searchStudCommand;
    private RelayCommand removeStudent;
    private RelayCommand resetFilter;
    private int indexResetFac = -1;
    private int indexResetCaf = -1;
    private int indexResetGrp = -1;
    private FacultetModel selectedFac;
    private CafedraModel selectedCaf;
    private GroupModel selectedGrp;
    private StudentModel selectedStd;
    private bool cafedraComboboxEnabled;
    private bool facultetComboboxEnabled;
    private bool groupComboboxEnabled;

    public ObservableCollection<FacultetModel> Facultets { get; set; }
    public ObservableCollection<StudentModel> Students { get; set; }
    public ObservableCollection<GroupModel> Groups { get; set; }
    public ObservableCollection<CafedraModel> Cafedras { get; set; }

    public SearchStudentsViewModel()
    {
        Facultets = BaseViewModel.Instance.Facultets;
        Cafedras = new ObservableCollection<CafedraModel>();
        Groups = new ObservableCollection<GroupModel>();
        Students = new ObservableCollection<StudentModel>();
        CafedraComboboxEnabled = true;
        FacultetComboboxEnabled = true;
        GroupComboboxEnabled = true;    
        SearchedStudents=new ObservableCollection<StudentModel>(Students);
        
        foreach (var a in Facultets)
        {
            if (a.Cafedra == null)
                a.Cafedra = new ObservableCollection<CafedraModel>();
            foreach (var b in a.Cafedra)
            {
                Cafedras.Add(b);
            }
        }
        foreach (var a in Cafedras)
        {
            if (a.Groups != null)
                foreach (var b in a.Groups)
                {
                    Groups.Add(b);
                }
        }
        foreach (var a in Groups)
        {
            if (a.Student != null)
                foreach (var b in a.Student)
                {
                    Students.Add(b);
                }
        }
        NewMethod();
    }

    public FacultetModel SelectedFacultet
    {
        get => selectedFac;
        set
        {
            selectedFac = value;
            OnPropertyChanged();
            FilterUpdate();
        }
    }
    public CafedraModel SelectedCafedra
    {
        get => selectedCaf;
        set
        {
            selectedCaf = value;
            OnPropertyChanged();
            FilterUpdate();
        }
    }
    public GroupModel SelectedGroup
    {
        get => selectedGrp;
        set
        {
            selectedGrp = value;
            OnPropertyChanged();
            FilterUpdate();
        }
    }
    public StudentModel SelectedStudent
    {
        get => selectedStd;
        set
        {
            selectedStd = value;
            OnPropertyChanged();

        }
    }
    public bool CafedraComboboxEnabled 
    {
        get => cafedraComboboxEnabled;
        set 
        {
            cafedraComboboxEnabled = value;
            OnPropertyChanged(); 
        } 
    }
    public bool FacultetComboboxEnabled 
    {
        get => facultetComboboxEnabled;
        set
        { 
            facultetComboboxEnabled = value;
            OnPropertyChanged(); 
        } 
    }
    public bool GroupComboboxEnabled 
    { 
        get => groupComboboxEnabled;
        set
        {
            groupComboboxEnabled = value;
            OnPropertyChanged();
        }
    }
    private void FilterUpdate()
    {
        if (SelectedFacultet != null && SelectedCafedra == null && SelectedGroup == null)
        {
            CafedraComboboxEnabled = false;
            GroupComboboxEnabled= false;
            if (SelectedFacultet != null)
            {
                Cafedras.Clear();
                if (SelectedFacultet.Cafedra == null)
                    SelectedFacultet.Cafedra = new ObservableCollection<CafedraModel>();
                foreach (var a in SelectedFacultet.Cafedra)
                {
                    Cafedras.Add(a);
                }
                Groups.Clear();
                foreach (var a in Cafedras)
                {
                    if (a != null)
                        if (a.Groups == null)
                            a.Groups = new ObservableCollection<GroupModel>();
                    foreach (var b in a?.Groups)
                    {
                        Groups.Add(b);
                    }
                }
                Students.Clear();
                foreach (var a in Groups)
                {
                    if (a != null)
                        if (a.Student == null)
                            a.Student = new ObservableCollection<StudentModel>();
                    foreach (var b in a?.Student)
                    {
                        Students.Add(b);
                    }
                }
                SearchedStudents = new ObservableCollection<StudentModel>(Students);
            }
        }
        if (SelectedFacultet == null && SelectedCafedra != null && SelectedGroup == null) 
        {
            FacultetComboboxEnabled = false;
            GroupComboboxEnabled = false;
            if (SelectedCafedra != null)
            {
                if (SelectedCafedra.Groups == null)
                    SelectedCafedra.Groups = new ObservableCollection<GroupModel>();
                Groups.Clear();
                foreach (var a in SelectedCafedra.Groups)
                {
                    Groups.Add(a);
                }
                Students.Clear();
                foreach (var a in Groups)
                {
                    if (a != null)
                        if (a.Student == null)
                            a.Student = new ObservableCollection<StudentModel>();
                    foreach (var b in a?.Student)
                    {
                        Students.Add(b);  
                    }
                }
                SearchedStudents = new ObservableCollection<StudentModel>(Students);
            }
        }
        if (SelectedFacultet == null && SelectedCafedra == null && SelectedGroup != null) 
        {
            CafedraComboboxEnabled = false;
            FacultetComboboxEnabled = false;
            if (SelectedGroup != null)
            {
                if (SelectedGroup.Student == null)
                    SelectedGroup.Student= new ObservableCollection<StudentModel>();
                Students.Clear();
                foreach (var a in SelectedGroup.Student)
                {
                    Students.Add(a);
                }
                SearchedStudents = new ObservableCollection<StudentModel>(Students);
            }
        } 
        /*if (SelectedFacultet == null && SelectedCafedra != null && SelectedGroup != null)
        {
            if (SelectedGroup != null)
            {
                
                FacultetComboboxEnabled = false;
                Students.Clear();
                

                //Groups.Clear();
                
                
                if (SelectedCafedra.Groups == null)
                {

                    SelectedCafedra.Groups = new ObservableCollection<GroupModel>();
                }
                Groups=new ObservableCollection<GroupModel>();

                foreach (var a in SelectedCafedra.Groups)
                {

                    Groups.Add(a);
                }
                
                if (SelectedGroup.Student == null)
                    SelectedGroup.Student = new ObservableCollection<StudentModel>();
                foreach (var a in SelectedGroup.Student)
                {
                    Students.Add(a);
                }  
            }
        } */
    }
    public int IndexResetFac
    {
        get => indexResetFac;
        set
        {
            indexResetFac = value;
            OnPropertyChanged("indexResetFac");
        }
    }
    public int IndexResetCaf
    {
        get => indexResetCaf;
        set
        {
            indexResetCaf = value;
            OnPropertyChanged("indexResetCaf");
        }
    }
    public int IndexResetGrp
    {
        get => indexResetGrp;
        set
        {
            indexResetGrp = value;
            OnPropertyChanged("indexResetGrp");
        }
    }
    public RelayCommand ResetFilter
    {
        get
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return resetFilter ??= new RelayCommand(obj =>
            {
                NewMethod();
            }/*, (obj) => (IndexResetFac != -1 || IndexResetCaf != -1 || IndexResetGrp != -1)*/);
        }
    }
    private void NewMethod()
    {
        IndexResetFac = -1;
        IndexResetCaf = -1;
        IndexResetGrp = -1;
        Facultets = BaseViewModel.Instance.Facultets;
        Cafedras.Clear();
        Groups.Clear();
        Students.Clear();
        CafedraComboboxEnabled = true;
        FacultetComboboxEnabled = true;
        GroupComboboxEnabled = true;
        foreach (var a in Facultets)
        {
            if (a.Cafedra != null)
                foreach (var b in a.Cafedra)
                {
                    Cafedras.Add(b);
                }
        }
        foreach (var a in Cafedras)
        {
            if (a.Groups != null)
                foreach (var b in a.Groups)
                {
                    Groups.Add(b);
                }
        }
        foreach (var a in Groups)
        {
            if (a.Student != null)
                foreach (var b in a.Student)
                {
                    Students.Add(b);
                }
        }
        SearchedStudents = new ObservableCollection<StudentModel>(Students);
    }
    public ObservableCollection<StudentModel> SearchedStudents
    {
        get => searchedStudents;
        set
        {
            searchedStudents = value;
            OnPropertyChanged("SearchedStudents");
        }
    }
    public string SearchSearchStudents
    {
        get => searchSearchStudents;
        set { searchSearchStudents = value; OnPropertyChanged("SearchStudents"); }
    }
    private void SearchStud()
    {
        if (string.IsNullOrWhiteSpace(SearchSearchStudents))
        {
            SearchedStudents = new ObservableCollection<StudentModel>(Students);
        }
        else
        {
            SearchedStudents = new ObservableCollection<StudentModel>(Students.Where(f => f.Name.Contains(SearchSearchStudents, StringComparison.OrdinalIgnoreCase)));
        }
    }
    public RelayCommand SearchStudCommand
    {
        get
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return searchStudCommand ??= new RelayCommand(obj =>
            {
                SearchStud();
            });
        }
    }
    public RelayCommand RemoveStudent
    {
        get
        {
            // ReSharper disable once NullCoalescingConditionIsAlwaysNotNullAccordingToAPIContract
            return removeStudent ??= new RelayCommand(obj =>
            {
                if (obj is not StudentModel std) return;
                Students.Remove(std);
                SearchedStudents = Students;
                BaseViewModel.db.Students.Remove(std);
                BaseViewModel.db.SaveChanges();
                BaseViewModel.Instance.Students.Remove(std);

                if (Students?.Count > 0)
                    SelectedStudent = Students[^1];
            }, (obj) => (Students.Count > 0 && SelectedStudent != null));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}