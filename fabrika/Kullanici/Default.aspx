<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Kullanici_Default" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
      <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <asp:Button ID="btnYeniEkle" runat="server" Text="+ Yeni Kullanici Ekle" CssClass="btn btn-shadow btn-success" OnClick="btnYeniEkle_Click" />
 

                </header>
                <div class="panel-body">

                    <asp:HiddenField ID="HiddenFieldSirketID" runat="server" />
                    <dx:ASPxGridView ID="ASPxGridViewKullanicilar" ClientInstanceName="ASPxGridViewKullanicilar" runat="server" AutoGenerateColumns="False" DataSourceID="LinqDataSourceKullanicilar" KeyFieldName="KullaniciID" Width="100%" Theme="Default">
                        <SettingsEditing EditFormColumnCount="1" Mode="PopupEditForm">
                        </SettingsEditing>
                        <SettingsPager Mode="ShowAllRecords"></SettingsPager>
                        <SettingsSearchPanel Visible="True" />
                        <SettingsText SearchPanelEditorNullText="Arama istediğiniz" PopupEditFormCaption="Yeni Kayit" CommandCancel="Vazgeç" CommandDelete="Sil" CommandEdit="Düzenle" CommandNew="Yeni Ekle" CommandUpdate="Güncelle" EmptyDataRow="Henüz eklenmiş bilgi yok."></SettingsText>
                        <Settings ShowFilterRow="True" />
                        <SettingsPopup>
                            <EditForm HorizontalAlign="WindowCenter" AllowResize="True" Modal="True" Width="400px" VerticalAlign="WindowCenter" />
                        </SettingsPopup>
                        <Columns>
                            <dx:GridViewDataTextColumn FieldName="KullaniciID" VisibleIndex="0" ReadOnly="True">
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataHyperLinkColumn FieldName="KullaniciID" ReadOnly="True" VisibleIndex="1" Width="50px">
                                <PropertiesHyperLinkEdit NavigateUrlFormatString="YeniKullanici.aspx?KullaniciID={0}" Text="Kullanci Güncelle">
                                </PropertiesHyperLinkEdit>
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataHyperLinkColumn>
                            <dx:GridViewDataTextColumn FieldName="SirketID" VisibleIndex="1">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="AdSoyad" VisibleIndex="2">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Sifre" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Telefon" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="YetkiID" VisibleIndex="6">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataCheckColumn FieldName="Aktif" VisibleIndex="7">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataDateColumn FieldName="OlusturmaTarihi" VisibleIndex="8">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataDateColumn FieldName="SonGirisTarihi" VisibleIndex="9">
                            </dx:GridViewDataDateColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <asp:LinqDataSource ID="LinqDataSourceKullanicilar" runat="server" ContextTypeName="FabrikaDataClassesDataContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntityTypeName="" TableName="Kullanicilars" Where="SirketID == @SirketID">
                        <WhereParameters>
                            <asp:ControlParameter ControlID="HiddenFieldSirketID" Name="SirketID" PropertyName="Value" Type="Int32" />
                        </WhereParameters>
                        <UpdateParameters>
                            <asp:SessionParameter Name="SirketID" SessionField="SirketID" Type="Int32" />
                            <asp:Parameter DefaultValue="True" Name="Durum" Type="Boolean" />
                        </UpdateParameters>
                        <InsertParameters>
                            <asp:SessionParameter Name="SirketID" SessionField="SirketID" Type="Int32" />
                            <asp:Parameter DefaultValue="True" Name="Durum" Type="Boolean" />
                        </InsertParameters>
                    </asp:LinqDataSource>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

