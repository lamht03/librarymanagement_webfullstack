import React from "react";
import "../../Assets/Styles/Layout/Footer.css";
import Logo from "../../Assets/Images/Img_Logo_LoginForm_Tdu.png";

const Footer = () => {
  return (
    <footer className="Footer" id="settings">
      {/* Phần bên trái */}
      <div className="Footer-Left">
        <div className="Footer-Contact">
          <div className="Contact-Item">
            <i className="fas fa-phone-alt"></i> 0934 078 668 - 0243 386 1601
          </div>
          <div className="Contact-Item">
            <i className="fas fa-envelope"></i>
            <a href="mailto:daihocthanhdo@thanhdouni.edu.vn">
              daihocthanhdo@thanhdouni.edu.vn
            </a>
          </div>
          <div className="Contact-Item">
            <i className="fas fa-map-marker-alt"></i>
            <a
              href="https://www.google.com/maps/place/Tr%C6%B0%E1%BB%9Dng+%C4%90%E1%BA%A1i+h%E1%BB%8Dc+Th%C3%A0nh+%C4%90%C3%B4/@21.0639708,105.7226073,17z/data=!3m1!4b1!4m6!3m5!1s0x3134544d67192907:0xaebb53aa417a4231!8m2!3d21.0639708!4d105.7226073!16s%2Fg%2F120l36ss?hl=vi-VN&entry=ttu&g_ep=EgoyMDI0MTExOS4yIKXMDSoASAFQAw%3D%3D"
              style={{ marginLeft: "3px" }}
            >
              Kim Chung - Hoài Đức - Hà Nội
            </a>
          </div>
        </div>
      </div>

      {/* Phần ở giữa */}
      <div className="Footer-Center">
        <a href="https://thanhdo.edu.vn/" className="Footer-Logo">
          <img src={Logo} alt="TDU Logo" className="Footer-Img" />
        </a>
        <p className="Footer-Copyright">
          © {new Date().getFullYear()} Library Management. All rights reserved.
        </p>
        <p className="Footer-Links">
          <a href="https://thanhdo.edu.vn/" className="Footer-Link">
            Privacy Policy
          </a>{" "}
          |{" "}
          <a href="https://thanhdo.edu.vn/" className="Footer-Link">
            Terms of Use
          </a>
        </p>
      </div>

      {/* Phần bên phải: Quick Links */}
      <div className="Footer-Right">
        <ul className="Footer-QuickLinks">
          <li>
            <a href="https://thanhdo.edu.vn/" className="Footer-Link">
              <i className="fas fa-info-circle"></i> About Us
            </a>
          </li>
          <li>
            <a href="https://thanhdo.edu.vn/" className="Footer-Link">
              <i className="fas fa-question-circle"></i> FAQs
            </a>
          </li>
          <li>
            <a href="https://thanhdo.edu.vn/" className="Footer-Link">
              <i className="fas fa-envelope"></i> Contact
            </a>
          </li>
        </ul>
      </div>
    </footer>
  );
};

export default Footer;
