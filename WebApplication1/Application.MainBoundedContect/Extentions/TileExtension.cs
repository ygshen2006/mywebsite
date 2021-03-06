﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.MainBoundedContect.Enums;
using Application.MainBoundedContect.Services.Tile;
using Application.MainBoundedContect.ViewModel.Tiles;
using Domain.MainBoundedContext.Tiles.Aggregates;
using Domain.MainBoundedContext.Logics;
using Application.MainBoundedContect.ViewModel.Report;
using Domain.MainBoundedContext.Reports.FilterField;
using Application.MainBoundedContect.ViewModel.TeamSites;
using Application.MainBoundedContect.ViewModel.SiteAdministration;
using Application.MainBoundedContect.ViewModel.Users;
using System.Web.Script.Serialization;

namespace Application.MainBoundedContect.Extentions
{
    public static class TileExtension
    {
        /// <summary>
        /// Convert from an AppTile type to Tile
        /// </summary>
        /// <param name="apptile"></param>
        /// <returns></returns>
        public static Tile ToTile(this AppTile apptile)
        {
            return new Tile()
            {
                Id = apptile.Id != null ? apptile.Id.Value : 0,
                BackgroundColor = apptile.BackgroundColor,
                BackgroundImage = apptile.BackgroundImage,
                Height = apptile.Height,
                Icon = apptile.Icon,
                ImageOverlayColor = apptile.ImageOverlayColor,
                IsCustomized = apptile.IsCustomized,
                Left = apptile.Left,
                LogicStringType = (Int16)apptile.logicType,
                OwnerTeamSiteId = apptile.OwnerTeamSiteId,
                ReportCountStyle = apptile.ReportCountStyle,
                TileType = (Int32)apptile.TileType,
                Title = apptile.Title,
                Top = apptile.Top,
                Width = apptile.Width
               
            };
        }

        /// <summary>
        /// Convert a tile type to apptile type
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public static AppTile ToAppTile(this Tile tile)
        {
            AppTile appTile = null;

            #region Initial
            if (tile.Id <= 3)
            {
                if (tile.Id == SystemDefinedTile.MyReports_MySubscriptions.SystemDefinedTileId)
                {
                    appTile = new AppMySubscriptionsTile();
                }
                else if (tile.Id == SystemDefinedTile.MyReports_MyReports.SystemDefinedTileId)
                {
                    appTile = new AppMyReportsTile();
                }
                else if (tile.Id == SystemDefinedTile.MyReports_MySubscriptions.SystemDefinedTileId)
                {
                    appTile = new AppMySubscriptionsTile();
                }
                //else
                //{
                //    throw new Exception(String.Format("DO NOT support the {0}", tile.Id.ToString()));
                //}
            }
            else
            {
                appTile = new AppTeamSiteCustomizedTile();
            }

            #endregion

            appTile.Id = tile.Id;
            appTile.Title = tile.Title;
            appTile.IsCustomized = tile.IsCustomized;
            appTile.Top = tile.Top;
            appTile.Left = tile.Left;
            appTile.Width = tile.Width;
            appTile.Height = tile.Height;
            appTile.BackgroundColor = tile.BackgroundColor;
            appTile.Icon = tile.Icon;
            appTile.BackgroundImage = tile.BackgroundImage;
            appTile.ImageOverlayColor = tile.ImageOverlayColor;
            appTile.ReportCountStyle = tile.ReportCountStyle;
            appTile.OwnerTeamSiteId = tile.OwnerTeamSiteId;
            appTile.Status = ChangeStatus.Normal;
            appTile.TileType = (TileType)(tile.TileType);
            appTile.logicType = (LogicType)tile.LogicStringType;
            return appTile;
        }

        public static TileViewModel ToTileViewModel(this AppTile appTile)
        {
            TileViewModel tile = new TileViewModel();
            tile.id = appTile.Id.Value;
            tile.title = appTile.Title;
            tile.TileType = (Int32)appTile.TileType;
            tile.coordinateY = appTile.Top;
            tile.coordinateX = appTile.Left;
            tile.demensionX = appTile.Width;
            tile.demensionY = appTile.Height;
            tile.icon = appTile.Icon;
            tile.backgroundColor = appTile.BackgroundColor;
            tile.backgroundImage = appTile.BackgroundImage;
            tile.overlayColor = appTile.ImageOverlayColor;
            tile.shownCount = appTile.ReportCountStyle;
            tile.countNum = appTile.ReportCount.ToString();
            tile.IsCustomized = appTile.IsCustomized;
            tile.OwnerTeamSiteId = appTile.OwnerTeamSiteId;
            tile.LogicType = appTile.logicType.ToString();
            tile.LogicString = GetlogicStringFromLogic(appTile);

            return tile;
        }


        private static string GetlogicStringFromLogic(AppTile appTile)
        {
            string logicString = string.Empty;

            switch (appTile.logicType)
            {
                case LogicType.Selected:
                    logicString = string.Join(",", (appTile.BasicLogic as IN<int>).FieldValue.GetValue());
                    break;
                case LogicType.Filtered:

                    List<Logic> elements = (appTile.BasicLogic as AND).LogicElements;
                    if (elements != null && elements.Count > 0)
                    {
                        #region Get ReportFilter from Logic
                        ReportFilter filer = new ReportFilter();
                        foreach (var t in elements)
                        {
                            if (t as IN<int> != null)
                            {
                                if ((t as IN<int>).Field is TagId)
                                {
                                    filer.TagsIdCollection = (t as IN<int>).FieldValue.GetValue();
                                }
                                if ((t as IN<int>).Field is SubCategoryId)
                                {
                                    filer.SubCategoryIdCollection = (t as IN<int>).FieldValue.GetValue();
                                }
                            }

                            if ((t as IN<string>) != null)
                            {
                                if ((t as IN<string>).Field is ReportOwnerId)
                                {
                                    filer.OwnerIdCollection = (t as IN<string>).FieldValue.GetValue();
                                }
                            }
                            
                        }
                        #endregion

                        #region Get TileFilterListViewModel from ReportFilter
                        TileFilterListViewModel vm = new TileFilterListViewModel();

                        if (filer.TagsIdCollection != null && filer.TagsIdCollection.Count > 0)
                        {
                            vm.Tag = filer.TagsIdCollection.Select(id => new AppTeamTag { Id = id });
                        }
                        if (filer.SubCategoryIdCollection != null && filer.SubCategoryIdCollection.Count > 0)
                        {
                            vm.SubCategory = filer.SubCategoryIdCollection.Select(id => new AppCategory { Id = id });
                        }

                        if (filer.OwnerIdCollection != null && filer.OwnerIdCollection.Count > 0)
                        {
                            vm.Owner = filer.OwnerIdCollection.Select(id => new UserLoginApp { Id = id });
                        }
                        #endregion

                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        logicString = jss.Serialize(vm);

                    }
                    break;
                case LogicType.Tagged:
                    logicString = string.Join(",", (appTile.BasicLogic as IN<int>).FieldValue.GetValue());
                    break;
                default:
                    break;
            }
            return logicString;
        }
    }
}
