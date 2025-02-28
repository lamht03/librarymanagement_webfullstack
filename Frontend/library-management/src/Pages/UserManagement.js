import React from "react";
import Header from "../Components/Layout/Header";
import Footer from "../Components/Layout/Footer";
import Sidebar from "../Components/Layout/Sidebar";
import Content from "../Components/Layout/Content/UserContent";
import "../Assets/Styles/Pages/UserManagement.css"

const UserManagement = () => {
  return (
    <div className="layout">
      <Header />
      <div className="main">
        <Sidebar />
        <Content />
      </div>
      <Footer />
    </div>
  );
};

export default UserManagement;
