import React from 'react';
import { Navigate } from 'react-router-dom';

const ProtectedRoute = ({ children }) => {
  // Kiểm tra trạng thái đăng nhập từ localStorage
  const isAuthenticated = localStorage.getItem("isAuthenticated");

  if (!isAuthenticated) {
    // Nếu chưa đăng nhập, chuyển hướng về trang đăng nhập
    return <Navigate to="/" />;
  }

  return children; // Nếu đã đăng nhập, hiển thị trang yêu cầu bảo vệ
};

export default ProtectedRoute;
