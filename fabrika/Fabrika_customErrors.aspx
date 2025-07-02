<%@ Page Title="Hata" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Fabrika_customErrors.aspx.cs" Inherits="fabrika_Fabrika_customErrors" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function startCountdown(totalSeconds) {
            function updateCountdown() {
                if (totalSeconds < 0) return;
                var days = Math.floor(totalSeconds / (24 * 3600));
                var hours = Math.floor((totalSeconds % (24 * 3600)) / 3600);
                var minutes = Math.floor((totalSeconds % 3600) / 60);
                var seconds = totalSeconds % 60;
                document.getElementById('lblDays').innerText = days < 10 ? '0' + days : days;
                document.getElementById('lblHours').innerText = hours < 10 ? '0' + hours : hours;
                document.getElementById('lblMinutes').innerText = minutes < 10 ? '0' + minutes : minutes;
                document.getElementById('lblSeconds').innerText = seconds < 10 ? '0' + seconds : seconds;
                totalSeconds--;
                setTimeout(updateCountdown, 1000);
            }
            updateCountdown();
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="min-height: 80vh; display: flex; align-items: center; justify-content: center;">
        <div style="background: #fffbe6; border: 2px solid #ffe066; border-radius: 18px; box-shadow: 0 6px 32px 0 rgba(0,0,0,0.10); padding: 40px 32px; width: 100%; max-width: 420px; text-align: center;">
            <header style="display: flex; align-items: center; justify-content: center; gap: 12px; margin-bottom: 24px;">
                <span style="font-size:2.5rem; color: #ffd43b;">⚠️</span>
                <h2 style="font-size:2rem; font-weight:700; color:#a07900; margin:0;">
                    <asp:Label ID="lblErrorCode" runat="server" Text="500" />
                </h2>
                <span style="font-size:2.5rem; color: #ffd43b;">⚠️</span>
            </header>
            <div style="color:#a07900; font-size:1.3rem; font-weight:600; margin-bottom: 16px;">
                <asp:Label ID="lblErrorMessage" runat="server" Text="Üzgünüz, bir hata oluştu!" />
            </div>
            <p style="color:#a07900; font-size:1.1rem; margin-bottom: 24px;">
                <asp:Label ID="lblErrorDetails" runat="server" Text="İşleminiz sırasında beklenmeyen bir hata oluştu." />
            </p>
            <div style="display: flex; align-items: center; justify-content: center; margin-bottom: 12px;">
                <span style="font-size:1.3rem; color:#ffd43b;">⏰</span>
                <span style="color:#a07900; font-weight:600; margin-left:8px; font-size:1.1rem;">Sayfanın Hazır Olmasına Kalan Süre:</span>
            </div>
            <section style="display: grid; grid-template-columns: repeat(4, 1fr); gap: 12px; margin-bottom: 24px;">
                <article style="display: flex; flex-direction: column; align-items: center; background: #fff; padding: 16px 8px; border-radius: 10px; border: 1.5px solid #ffe066; box-shadow: 0 2px 8px 0 rgba(255,212,67,0.08);">
                    <span id="lblDays" style="font-size:2rem; font-weight:700; color:#ffd43b; letter-spacing:1px;">00</span>
                    <span style="font-size:0.95rem; color:#a07900; font-weight:500;">Gün</span>
                </article>
                <article style="display: flex; flex-direction: column; align-items: center; background: #fff; padding: 16px 8px; border-radius: 10px; border: 1.5px solid #ffe066; box-shadow: 0 2px 8px 0 rgba(255,212,67,0.08);">
                    <span id="lblHours" style="font-size:2rem; font-weight:700; color:#ffd43b; letter-spacing:1px;">00</span>
                    <span style="font-size:0.95rem; color:#a07900; font-weight:500;">Saat</span>
                </article>
                <article style="display: flex; flex-direction: column; align-items: center; background: #fff; padding: 16px 8px; border-radius: 10px; border: 1.5px solid #ffe066; box-shadow: 0 2px 8px 0 rgba(255,212,67,0.08);">
                    <span id="lblMinutes" style="font-size:2rem; font-weight:700; color:#ffd43b; letter-spacing:1px;">00</span>
                    <span style="font-size:0.95rem; color:#a07900; font-weight:500;">Dakika</span>
                </article>
                <article style="display: flex; flex-direction: column; align-items: center; background: #fff; padding: 16px 8px; border-radius: 10px; border: 1.5px solid #ffe066; box-shadow: 0 2px 8px 0 rgba(255,212,67,0.08);">
                    <span id="lblSeconds" style="font-size:2rem; font-weight:700; color:#ffd43b; letter-spacing:1px;">00</span>
                    <span style="font-size:0.95rem; color:#a07900; font-weight:500;">Saniye</span>
                </article>
            </section>
            <div style="display: flex; gap: 12px; justify-content: center;">
                <asp:Button ID="btnRetry" runat="server" Text="Tekrar Dene" CssClass="btn btn-warning" OnClick="btnRetry_Click" />
                <asp:Button ID="btnHome" runat="server" Text="Ana Sayfa" CssClass="btn btn-primary" OnClick="btnHome_Click" />
            </div>
        </div>
    </div>
    <script type="text/javascript">
        window.onload = function() {
            startCountdown(<%= RemainingSeconds.ToString() %>);
        };
    </script>
</asp:Content>
