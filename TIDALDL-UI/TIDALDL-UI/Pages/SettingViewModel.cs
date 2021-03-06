﻿using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIDALDL_UI.Else;
using AIGS.Common;
using System.IO;
using Tidal;
using System.Windows.Forms;
using AIGS.Helper;
using System.Windows.Controls;

namespace TIDALDL_UI.Pages
{
    public class SettingViewModel: Stylet.Screen
    {
        public string OutputDir { get; set; }
        public int    ThreadNum { get; set; }
        public int    SearchNum { get; set; }
        public int    SelectQualityIndex { get; set; }
        public int    SelectResolutionIndex { get; set; }
        public bool   OnlyM4a { get; set; }
        public bool   AddHyphen { get; set; }
        public bool   UseTrackNumber { get; set; }
        public bool   ToChinese { get; set; }
        public bool   CheckExist { get; set; }
        public bool   ArtistBeforeTitle { get; set; }
        public string MaxFileName { get; set; } = "60";
        public string MaxDirName { get; set; } = "60";

        public bool AddExplicitTag { get; set; }
        public bool IncludeEPSingle { get; set; }
        public int  AddYearIndex { get; set; }
        public bool SaveCovers { get; set; }

        public bool CheckCommon { get; set; } = true;
        public bool CheckTrack { get; set; } = false;
        public bool CheckVideo { get; set; } = false;

        public List<string> QualityList { get; set; }
        public List<string> ResolutionList { get; set; }
        
        public SettingViewModel()
        {
            RefreshSetting();
        }

        public string CheckMaxName(string sMaxNum)
        {
            int iTmp;
            if (!int.TryParse(sMaxNum, out iTmp))
                iTmp = 60;
            if (iTmp < 50 || iTmp > 100)
                iTmp = 60;
            return iTmp.ToString();
        }

        public void RefreshSetting()
        {
            OutputDir             = Config.OutputDir();
            OnlyM4a               = Config.OnlyM4a();
            AddExplicitTag        = Config.AddExplicitTag();
            IncludeEPSingle       = Config.IncludeEP();
            AddHyphen             = Config.AddHyphen();
            SaveCovers            = Config.SaveCovers();
            ToChinese             = Config.ToChinese();
            MaxFileName           = CheckMaxName(Config.MaxFileName());
            MaxDirName            = CheckMaxName(Config.MaxDirName());
            CheckExist            = Config.CheckExist();
            ArtistBeforeTitle     = Config.ArtistBeforeTitle();
            AddYearIndex          = Config.AddYear();
            ThreadNum             = AIGS.Common.Convert.ConverStringToInt(Config.ThreadNum()) - 1;
            SearchNum             = AIGS.Common.Convert.ConverStringToInt(Config.SearchNum()) / 10 - 1;
            QualityList           = TidalTool.getQualityList();
            ResolutionList        = TidalTool.getResolutionList();
            SelectQualityIndex    = QualityList.IndexOf(Config.Quality().ToUpper());
            SelectResolutionIndex = ResolutionList.IndexOf(Config.Resolution().ToUpper());
            UseTrackNumber        = Config.UseTrackNumber();

            if (SelectQualityIndex < 0)
                SelectQualityIndex = 0;
            if (SelectResolutionIndex < 0)
                SelectResolutionIndex = 0;
            if (ThreadNum < 0)
                ThreadNum = 0;
            if (SearchNum < 0 || SearchNum > 5)
                SearchNum = 0;
        }

        public void SetOutputDir()
        {
            FolderBrowserDialog openFileDialog = new FolderBrowserDialog();
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                OutputDir = openFileDialog.SelectedPath;
        }

        public void Confirm()
        {
            Config.ThreadNum((ThreadNum + 1).ToString());
            Config.SearchNum(((SearchNum + 1)*10).ToString());
            Config.OnlyM4a(OnlyM4a.ToString());
            Config.AddExplicitTag(AddExplicitTag.ToString());
            Config.SaveCovers(SaveCovers.ToString());
            Config.IncludeEP(IncludeEPSingle.ToString());
            Config.ToChinese(ToChinese.ToString());
            Config.CheckExist(CheckExist.ToString());
            Config.ArtistBeforeTitle(ArtistBeforeTitle.ToString());
            Config.AddHyphen(AddHyphen.ToString());
            Config.AddYear(AddYearIndex);
            Config.Quality(QualityList[SelectQualityIndex].ToLower());
            Config.Resolution(ResolutionList[SelectResolutionIndex]);
            Config.OutputDir(OutputDir);
            Config.UseTrackNumber(UseTrackNumber.ToString());
            Config.MaxFileName(CheckMaxName(MaxFileName));
            Config.MaxDirName(CheckMaxName(MaxDirName));

            TidalTool.SetSearchMaxNum(int.Parse(Config.SearchNum()));
            ThreadTool.SetThreadNum(ThreadNum + 1);
            RequestClose();                                                                 
        }

    }
}

