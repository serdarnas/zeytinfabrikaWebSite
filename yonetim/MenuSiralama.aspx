<%@ Page Title="" Language="C#" MasterPageFile="~/yonetim/YonetimMasterPage.master" AutoEventWireup="true" CodeFile="MenuSiralama.aspx.cs" Inherits="yonetim_MenuSiralama" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <%--<link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/nestable/jquery.nestable.css" />--%>
    <style>
        /* Nestable List 3 özel stilleri */
        .dd3-row {
            display: flex;
            align-items: center;
            width: 100%;
            gap: 0;
        }
        .dd-handle.dd3-handle {
            flex-shrink: 0;
            margin-right: 10px;
            width: 30px;
            height: 30px;
            background: #ccc;
            display: inline-block;
            vertical-align: middle;
            cursor: move;
        }
        .dd3-content {
            flex: 1 1 0;
            display: flex;
            align-items: center;
            padding: 5px 12px;
            background: #fafafa;
            border: 1px solid #ddd;
            margin: 5px 0;
            font-size: 16px;
            gap: 10px;
            width: 100%;
            box-sizing: border-box;
            min-width: 0;
        }
        .menu-url {
            color: #888;
            font-size: 12px;
            margin-left: 15px;
            word-break: break-all;
            flex-shrink: 1;
            overflow: hidden;
            text-overflow: ellipsis;
            white-space: nowrap;
        }
        @media (max-width: 600px) {
            .dd3-content {
                font-size: 14px;
                padding: 5px 6px;
            }
            .menu-url {
                font-size: 10px;
                margin-left: 5px;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    Menü Sıralama (Nestable List 3)
                </header>
                <div class="panel-body">
                    <div class="dd" id="nestable_list_3">
                        <asp:Literal ID="ltMenu" runat="server"></asp:Literal>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>

<asp:Content ID="scripts" ContentPlaceHolderID="scripts" runat="server">
    <script>
        $(document).ready(function () {
            $('#nestable_list_3').nestable();
            $('#nestable_list_3').on('change', function () {
                var order = $('#nestable_list_3').nestable('serialize');
                PageMethods.GuncelleSira(order, function (response) {
                    // Başarı mesajı veya başka bir işlem
                }, function (err) {
                    alert('Sıralama kaydedilemedi!');
                });
            });
        });
    </script>
</asp:Content>

