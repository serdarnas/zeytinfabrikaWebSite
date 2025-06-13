<%@ Page Title="Üretim İçin Makine ve Malaksör Seçimi" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="PartiMakineSecimi.aspx.cs" Inherits="fabrika_Zeytinyagi_PartiMakineSecimi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .form-group label {
            font-weight: bold;
        }
        .panel-heading {
            font-size: 1.2em;
        }
        .text-danger {
            display: block; /* Ensure validators take space */
            margin-top: 5px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header"><i class="fa fa-cogs"></i> Üretim İçin Makine ve Malaksör Seçimi</h3>
            <ol class="breadcrumb">
                <li><i class="fa fa-home"></i><a href="../Default.aspx">Ana Sayfa</a></li>
                <li><i class="fa fa-industry"></i>Zeytinyağı Üretimi</li>
                <li class="active"><i class="fa fa-cogs"></i>Makine Seçimi</li>
            </ol>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-8 col-lg-offset-2">
            <section class="panel">
                <header class="panel-heading">
                    Üretim Parti ID: <asp:Label ID="lblUretimID" runat="server" Font-Bold="true"></asp:Label>
                </header>
                <div class="panel-body">
                    <asp:Panel ID="pnlMessage" runat="server" Visible="false">
                        <div class="alert fade in" id="divMessage" runat="server">
                            <button data-dismiss="alert" class="close close-sm" type="button">
                                <i class="icon-remove"></i>
                            </button>
                            <strong id="strongMessageTitle" runat="server"></strong> <span id="spanMessage" runat="server"></span>
                        </div>
                    </asp:Panel>

                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Zeytinyağı Ana Makinası:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlZeytinyagiMakinasi" runat="server" CssClass="form-control m-bot15" OnSelectedIndexChanged="ddlZeytinyagiMakinasi_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvZeytinyagiMakinasi" runat="server" ControlToValidate="ddlZeytinyagiMakinasi"
                                    ErrorMessage="Zeytinyağı ana makinası seçimi zorunludur." InitialValue="0" Display="Dynamic" CssClass="text-danger" ValidationGroup="UretimBaslatValidation">* Ana makine seçin</asp:RequiredFieldValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Kullanılacak Malaksör:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlMalaksorler" runat="server" CssClass="form-control m-bot15">
                                </asp:DropDownList>
                                <asp:RequiredFieldValidator ID="rfvMalaksor" runat="server" ControlToValidate="ddlMalaksorler"
                                    ErrorMessage="Malaksör seçimi zorunludur." InitialValue="0" Display="Dynamic" CssClass="text-danger" ValidationGroup="UretimBaslatValidation">* Malaksör seçin</asp:RequiredFieldValidator>
                            </div>
                        </div>
                         <div class="form-group">
                            <label class="col-sm-4 control-label">İşlenecek Miktar (Kg):</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtKullanilanKg" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvKullanilanKg" runat="server" ControlToValidate="txtKullanilanKg"
                                    ErrorMessage="İşlenecek miktar boş bırakılamaz." Display="Dynamic" CssClass="text-danger" ValidationGroup="UretimBaslatValidation">* Miktar girin</asp:RequiredFieldValidator>
                                <asp:CompareValidator ID="cvKullanilanKgFormat" runat="server" ControlToValidate="txtKullanilanKg" Operator="DataTypeCheck" Type="Double"
                                    ErrorMessage="Lütfen geçerli bir sayısal değer girin (örn: 150.75)." Display="Dynamic" CssClass="text-danger" ValidationGroup="UretimBaslatValidation">* Geçerli sayı girin</asp:CompareValidator>
                                <asp:CompareValidator ID="cvKullanilanKgPozitif" runat="server" ControlToValidate="txtKullanilanKg" Operator="GreaterThan" ValueToCompare="0" Type="Double"
                                    ErrorMessage="İşlenecek miktar 0'dan büyük olmalıdır." Display="Dynamic" CssClass="text-danger" ValidationGroup="UretimBaslatValidation">* Miktar > 0 olmalı</asp:CompareValidator>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="col-sm-offset-4 col-sm-8">
                                <asp:Button ID="btnUretimiBaslat" runat="server" Text="Üretimi Başlat" CssClass="btn btn-primary" OnClick="btnUretimiBaslat_Click" ValidationGroup="UretimBaslatValidation" />
                            </div>
                        </div>
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="text-danger" ValidationGroup="UretimBaslatValidation" HeaderText="Lütfen aşağıdaki hataları düzeltin:" ShowMessageBox="false" ShowSummary="true"/>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
