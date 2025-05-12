<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Mustahsil_Default" %>

<%@ Register Assembly="DevExpress.Web.v16.1, Version=16.1.4.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <asp:Button ID="btnYeniEkle" runat="server" Text="+ Yeni Müstahsil Ekle" CssClass="btn btn-shadow btn-success" OnClick="btnYeniEkle_Click" />

                    <asp:HyperLink ID="btnExcelIndir" runat="server" NavigateUrl="Mustahsil_yukle_excel.aspx" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                        <i class="icon-file-excel"></i> Excel'den Müstahsil Yükle
                    </asp:HyperLink>

                </header>
                <div class="panel-body">

                    <asp:HiddenField ID="HiddenFieldSirketID" runat="server" />
                    <dx:ASPxGridView ID="ASPxGridViewMustahsil" ClientInstanceName="ASPxGridViewMustahsil" runat="server" AutoGenerateColumns="False" DataSourceID="LinqDataSourceMustahsiller" KeyFieldName="MustahsilID" Width="100%" Theme="Default">
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
                            <dx:GridViewCommandColumn ShowClearFilterButton="True" VisibleIndex="0">
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn FieldName="SirketID" Visible="False" VisibleIndex="3">
                            </dx:GridViewDataTextColumn>
                            
                            <dx:GridViewDataHyperLinkColumn FieldName="MustahsilID" ReadOnly="True" VisibleIndex="1" Width="50px">
                                <PropertiesHyperLinkEdit NavigateUrlFormatString="YeniMustahsil.aspx?id={0}" Text="Mustahsil Güncelle">
                                </PropertiesHyperLinkEdit>
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataHyperLinkColumn>

                            <dx:GridViewDataTextColumn FieldName="Ad" VisibleIndex="4">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Soyad" VisibleIndex="5">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Telefon" VisibleIndex="6" Visible="False">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Email" VisibleIndex="7" Visible="False">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="Adres" Visible="False" VisibleIndex="8">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn FieldName="TCKimlikNo" Visible="False" VisibleIndex="9">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataCheckColumn FieldName="Durum" Visible="False" VisibleIndex="10">
                            </dx:GridViewDataCheckColumn>
                            <dx:GridViewDataTextColumn FieldName="Notlar" Visible="False" VisibleIndex="11">
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataDateColumn FieldName="OlusturmaTarihi" Visible="False" VisibleIndex="12">
                            </dx:GridViewDataDateColumn>
                            <dx:GridViewDataHyperLinkColumn FieldName="MustahsilID" ReadOnly="True" VisibleIndex="2" Width="50px">
                                <PropertiesHyperLinkEdit NavigateUrlFormatString="MustahsilDetay.aspx?ID={0}" Text="Mustahsil Detay">
                                </PropertiesHyperLinkEdit>
                                <EditFormSettings Visible="False" />
                            </dx:GridViewDataHyperLinkColumn>
                            <dx:GridViewDataTextColumn FieldName="Bakiyesi" VisibleIndex="13">
                                <PropertiesTextEdit DisplayFormatString="0.00">
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                        </Columns>
                    </dx:ASPxGridView>
                    <asp:LinqDataSource ID="LinqDataSourceMustahsiller" runat="server" ContextTypeName="FabrikaDataClassesDataContext" EnableDelete="True" EnableInsert="True" EnableUpdate="True" EntityTypeName="" TableName="Mustahsillers" Where="Durum == @Durum &amp;&amp; SirketID == @SirketID">
                        <WhereParameters>
                            <asp:Parameter DefaultValue="True" Name="Durum" Type="Boolean" />
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

