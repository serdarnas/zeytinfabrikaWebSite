<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Depo.aspx.cs" Inherits="fabrika_Depo_Depo" %>

<%@ Register assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web" tagprefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
               <%-- <header class="panel-heading">
                    <h3>Dosya Yöneticisi</h3>
                </header>--%>
                <!-- İşlem Butonları -->
                <div class="panel-body">
                    <div class="btn-group">
                        <div class="col-lg-12">
                        <dx:ASPxFileManager ID="ASPxFileManager1" runat="server" Width="950px">
                            <settings rootfolder="~/" thumbnailfolder="~/Thumb/" EnableMultiSelect="True" />
                            <SettingsEditing AllowCopy="True" AllowCreate="True" AllowDelete="True" AllowDownload="True" AllowMove="True" AllowRename="True" />
                            <SettingsUpload AutoStartUpload="True">
                                <AdvancedModeSettings EnableMultiSelect="True" TemporaryFolder="~/Temp/UploadTemp/">
                                </AdvancedModeSettings>
                            </SettingsUpload>
                        </dx:ASPxFileManager>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

