using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web.UI.WebControls;
using BaiRong.Core;
using BaiRong.Core.Configuration;
using BaiRong.Core.Model.Enumerations;
using BaiRong.Core.Permissions;
using SiteServer.CMS.Core;
using SiteServer.CMS.Core.Security;
using SiteServer.CMS.Model;

namespace SiteServer.BackgroundPages.Admin
{
    public class PageRoleAddPublishmentSystemPermissions : BasePageCms
    {
        public CheckBoxList WebsitePermissions;
        public CheckBoxList ChannelPermissions;
        public Literal NodeTree;

        public PlaceHolder WebsitePermissionsPlaceHolder;
        public PlaceHolder ChannelPermissionsPlaceHolder;

        private string _appId;

        public static string GetRedirectUrl(int publishmentSystemId, string roleName)
        {
            var queryString = new NameValueCollection { { "PublishmentSystemID", publishmentSystemId.ToString() } };
            if (!string.IsNullOrEmpty(roleName))
            {
                queryString.Add("RoleName", roleName);
            }

            return PageUtils.GetAdminUrl(nameof(PageRoleAddPublishmentSystemPermissions), queryString);
        }

        private string GetNodeTreeHtml()
        {
            var htmlBuilder = new StringBuilder();
            var systemPermissionsInfoList = Session[PageRoleAdd.SystemPermissionsInfoListKey] as List<SystemPermissionsInfo>;
            if (systemPermissionsInfoList == null)
            {
                PageUtils.RedirectToErrorPage("超出时间范围，请重新进入！");
                return string.Empty;
            }
            var nodeIdList = new List<int>();
            foreach (var systemPermissionsInfo in systemPermissionsInfoList)
            {
                nodeIdList.AddRange(TranslateUtils.StringCollectionToIntList(systemPermissionsInfo.NodeIdCollection));
            }

            var treeDirectoryUrl = SiteServerAssets.GetIconUrl("tree");

            htmlBuilder.Append("<span id='ChannelSelectControl'>");
            var theNodeIdList = DataProvider.NodeDao.GetNodeIdListByPublishmentSystemId(PublishmentSystemId);
            var isLastNodeArray = new bool[theNodeIdList.Count];
            foreach (var theNodeId in theNodeIdList)
            {
                var nodeInfo = NodeManager.GetNodeInfo(PublishmentSystemId, theNodeId);
                htmlBuilder.Append(GetTitle(nodeInfo, treeDirectoryUrl, isLastNodeArray, nodeIdList));
                htmlBuilder.Append("<br/>");
            }
            htmlBuilder.Append("</span>");
            return htmlBuilder.ToString();
        }

        private string GetTitle(NodeInfo nodeInfo, string treeDirectoryUrl, IList<bool> isLastNodeArray, ICollection<int> nodeIdList)
        {
            var itemBuilder = new StringBuilder();
            if (nodeInfo.NodeId == PublishmentSystemId)
            {
                nodeInfo.IsLastNode = true;
            }
            if (nodeInfo.IsLastNode == false)
            {
                isLastNodeArray[nodeInfo.ParentsCount] = false;
            }
            else
            {
                isLastNodeArray[nodeInfo.ParentsCount] = true;
            }
            for (var i = 0; i < nodeInfo.ParentsCount; i++)
            {
                itemBuilder.Append(isLastNodeArray[i]
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_empty.gif\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_line.gif\"/>");
            }
            if (nodeInfo.IsLastNode)
            {
                itemBuilder.Append(nodeInfo.ChildrenCount > 0
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_plusbottom.gif\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_minusbottom.gif\"/>");
            }
            else
            {
                itemBuilder.Append(nodeInfo.ChildrenCount > 0
                    ? $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_plusmiddle.gif\"/>"
                    : $"<img align=\"absmiddle\" src=\"{treeDirectoryUrl}/tree_minusmiddle.gif\"/>");
            }

            var check = "";
            if (nodeIdList.Contains(nodeInfo.NodeId))
            {
                check = "checked";
            }

            var disabled = "";
            if (!IsOwningNodeId(nodeInfo.NodeId))
            {
                disabled = "disabled";
                check = "";
            }

            itemBuilder.Append(
                $@"<label class=""checkbox inline""><input type=""checkbox"" name=""NodeIDCollection"" value=""{nodeInfo
                    .NodeId}"" {check} {disabled}/> {nodeInfo.NodeName} &nbsp;<span style=""font-size:8pt;font-family:arial"" class=""gray"">({nodeInfo.ContentNum})</span></label>");

            return itemBuilder.ToString();
        }

        public void Page_Load(object sender, EventArgs e)
        {
            if (IsForbidden) return;

            var permissioins = PermissionsManager.GetPermissions(Body.AdministratorName);
            NodeTree.Text = GetNodeTreeHtml();

            if (IsPostBack) return;

            AdminManager.VerifyAdministratorPermissions(Body.AdministratorName, AppManager.Admin.Permission.AdminManagement);

            _appId = EPublishmentSystemTypeUtils.GetAppID(PublishmentSystemInfo.PublishmentSystemType);

            if (permissioins.IsSystemAdministrator)
            {
                var channelPermissions = PermissionConfigManager.GetChannelPermissionsOfApp(_appId);
                foreach (PermissionConfig permission in channelPermissions)
                {
                    if (permission.Name == AppManager.Cms.Permission.Channel.ContentCheckLevel1)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 1)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Cms.Permission.Channel.ContentCheckLevel2)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 2)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Cms.Permission.Channel.ContentCheckLevel3)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 3)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Cms.Permission.Channel.ContentCheckLevel4)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 4)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (permission.Name == AppManager.Cms.Permission.Channel.ContentCheckLevel5)
                    {
                        if (PublishmentSystemInfo.IsCheckContentUseLevel)
                        {
                            if (PublishmentSystemInfo.CheckContentLevel < 5)
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }
                    }
                    var listItem = new ListItem(permission.Text, permission.Name);
                    ChannelPermissions.Items.Add(listItem);
                }
            }
            else
            {
                ChannelPermissionsPlaceHolder.Visible = false;
                if (ProductPermissionsManager.Current.ChannelPermissionDict.ContainsKey(PublishmentSystemId))
                {
                    var channelPermissions = ProductPermissionsManager.Current.ChannelPermissionDict[PublishmentSystemId];
                    foreach (var channelPermission in channelPermissions)
                    {
                        foreach (PermissionConfig permission in PermissionConfigManager.Instance.ChannelPermissions)
                        {
                            if (permission.Name == channelPermission)
                            {
                                if (channelPermission == AppManager.Cms.Permission.Channel.ContentCheck)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel) continue;
                                }
                                else if (channelPermission == AppManager.Cms.Permission.Channel.ContentCheckLevel1)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 1) continue;
                                }
                                else if (channelPermission == AppManager.Cms.Permission.Channel.ContentCheckLevel2)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 2) continue;
                                }
                                else if (channelPermission == AppManager.Cms.Permission.Channel.ContentCheckLevel3)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 3) continue;
                                }
                                else if (channelPermission == AppManager.Cms.Permission.Channel.ContentCheckLevel4)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 4) continue;
                                }
                                else if (channelPermission == AppManager.Cms.Permission.Channel.ContentCheckLevel5)
                                {
                                    if (PublishmentSystemInfo.IsCheckContentUseLevel == false || PublishmentSystemInfo.CheckContentLevel < 5) continue;
                                }

                                ChannelPermissionsPlaceHolder.Visible = true;
                                var listItem = new ListItem(permission.Text, permission.Name);
                                ChannelPermissions.Items.Add(listItem);
                            }
                        }
                    }
                }
            }

            if (permissioins.IsSystemAdministrator)
            {
                var websitePermissions = PermissionConfigManager.GetWebsitePermissionsOfApp(_appId);
                foreach (PermissionConfig permission in websitePermissions)
                {
                    var listItem = new ListItem(permission.Text, permission.Name);
                    WebsitePermissions.Items.Add(listItem);
                }
            }
            else
            {
                WebsitePermissionsPlaceHolder.Visible = false;
                if (ProductPermissionsManager.Current.WebsitePermissionDict.ContainsKey(PublishmentSystemId))
                {
                    var websitePermissionList = ProductPermissionsManager.Current.WebsitePermissionDict[PublishmentSystemId];
                    foreach (var websitePermission in websitePermissionList)
                    {
                        foreach (PermissionConfig permission in PermissionConfigManager.Instance.WebsitePermissions)
                        {
                            if (permission.Name == websitePermission)
                            {
                                WebsitePermissionsPlaceHolder.Visible = true;
                                var listItem = new ListItem(permission.Text, permission.Name);
                                WebsitePermissions.Items.Add(listItem);
                            }
                        }
                    }
                }
            }

            var systemPermissionsInfoList = Session[PageRoleAdd.SystemPermissionsInfoListKey] as List<SystemPermissionsInfo>;
            if (systemPermissionsInfoList != null)
            {
                SystemPermissionsInfo systemPermissionsInfo = null;
                foreach (var publishmentSystemPermissionsInfo in systemPermissionsInfoList)
                {
                    if (publishmentSystemPermissionsInfo.PublishmentSystemId == PublishmentSystemId)
                    {
                        systemPermissionsInfo = publishmentSystemPermissionsInfo;
                        break;
                    }
                }
                if (systemPermissionsInfo == null) return;

                foreach (ListItem item in ChannelPermissions.Items)
                {
                    item.Selected = CompareUtils.Contains(systemPermissionsInfo.ChannelPermissions, item.Value);
                }
                foreach (ListItem item in WebsitePermissions.Items)
                {
                    item.Selected = CompareUtils.Contains(systemPermissionsInfo.WebsitePermissions, item.Value);
                }
            }
        }

        public override void Submit_OnClick(object sender, EventArgs e)
        {
            if (!Page.IsPostBack || !Page.IsValid) return;

            var systemPermissionsInfoList = Session[PageRoleAdd.SystemPermissionsInfoListKey] as List<SystemPermissionsInfo>;
            if (systemPermissionsInfoList != null)
            {
                var systemPermissionlist = new List<SystemPermissionsInfo>();
                foreach (var systemPermissionsInfo in systemPermissionsInfoList)
                {
                    if (systemPermissionsInfo.PublishmentSystemId == PublishmentSystemId)
                    {
                        continue;
                    }
                    systemPermissionlist.Add(systemPermissionsInfo);
                }

                var nodeIdList = TranslateUtils.StringCollectionToStringList(Request.Form["NodeIDCollection"]);
                if (nodeIdList.Count > 0 && ChannelPermissions.SelectedItem != null || WebsitePermissions.SelectedItem != null)
                {
                    var systemPermissionsInfo = new SystemPermissionsInfo
                    {
                        PublishmentSystemId = PublishmentSystemId,
                        NodeIdCollection = TranslateUtils.ObjectCollectionToString(nodeIdList),
                        ChannelPermissions =
                            ControlUtils.SelectedItemsValueToStringCollection(ChannelPermissions.Items),
                        WebsitePermissions =
                            ControlUtils.SelectedItemsValueToStringCollection(WebsitePermissions.Items)
                    };

                    systemPermissionlist.Add(systemPermissionsInfo);
                }

                Session[PageRoleAdd.SystemPermissionsInfoListKey] = systemPermissionlist;
            }

            PageUtils.Redirect(PageRoleAdd.GetReturnRedirectUrl(Body.GetQueryString("RoleName")));
        }

        public void Return_OnClick(object sender, EventArgs e)
        {
            PageUtils.Redirect(PageRoleAdd.GetReturnRedirectUrl(Body.GetQueryString("RoleName")));
        }
    }
}
