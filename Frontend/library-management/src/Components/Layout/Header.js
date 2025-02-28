import React, { useState } from "react";
import "../../Assets/Styles/Layout/Header.css";
import Avt from "../../Assets/Images/Img_Anya_Chibi.png";
import "@fortawesome/fontawesome-free/css/all.min.css";

const Header = () => {
  const [isProfileMenuOpen, setIsProfileMenuOpen] = useState(false);

  // Hàm xử lý khi nhấn vào ảnh đại diện
  const handleProfileMenuToggle = () => {
    setIsProfileMenuOpen((prev) => !prev); // Đảo trạng thái của menu
  };

    // Hàm xử lý khi nhấn vào mục trong menu (ví dụ: Account Info, Settings, Logout)
    const handleMenuItemClick = (item) => {
      console.log(item);  // Xử lý hành động cho từng mục trong menu
      setIsProfileMenuOpen(false);  // Đóng menu sau khi chọn
    };
  

  const handleLogout = () => {
    localStorage.removeItem("isAuthenticated");  // Xóa trạng thái đăng nhập
    window.location.href = "/"; 
  };

  return (
    <header className="Header">
      <div className="Header-Left">
        <div className="Header-Title2-Container">
          <div className="Header-Title2">
            <span className="Header-Title2-TDU">TDU</span>
            <span className="Header-Title2-Library">Library</span>
          </div>
        </div>
        <div className="Header-Icons">
          <i className="fas fa-home Header-Icon" title="Home"></i>
          <i className="fas fa-search Header-Icon" title="Search"></i>
          <i className="fas fa-phone-alt Header-Icon" title="Contact"></i>
          <i className="fas fa-cog Header-Icon" title="Settings"></i>
        </div>
      </div>
      <div className="Header-Center">
        <div className="Search-Bar">
          <select className="Search-Dropdown">
            <option value="books">📚 Books</option>
            <option value="authors">👤 Authors</option>
            <option value="categories">🗂️ Categories</option>
          </select>
          <input
            type="text"
            placeholder="Search for books, authors, or categories..."
            className="Header-Search"
          />
          <button className="Search-Button">
            <i className="fas fa-search" />
          </button>
        </div>
      </div>
      <div className="Header-Right">
        <div className="Header-Icons">
          <i className="fas fa-user Header-Icon" title="User Management"></i>
          <i className="fas fa-bell Header-Icon" title="Notification"></i>
          <i className="fas fa-exchange-alt Header-Icon" title="Transaction Management"></i>
          <i className="fas fa-credit-card Header-Icon" title="Payment Management"></i>
        </div>

        <div className="Profile" onClick={handleProfileMenuToggle}>
          <img
            src={Avt}
            alt="Profile"
            title="Profile"
            className="Profile-Img"
          />
          {isProfileMenuOpen && (
            <div className="Profile-Menu">
              <div className="Profile-Menu-Item" onClick={() => handleMenuItemClick("Account Info")}>
                <i className="fas fa-user-circle"></i> Account Info
              </div>
              <div className="Profile-Menu-Item" onClick={() => handleMenuItemClick("Settings")}>
                <i className="fas fa-cog"></i> Settings
              </div>
              <div className="Profile-Menu-Item" onClick={handleLogout}>
              <i className="fas fa-sign-out-alt"></i> Logout
              </div>
            </div>
          )}
        </div>
      </div>
    </header>
  );
};

export default Header;
