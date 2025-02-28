// src/api.js
import axios from 'axios';
import config from './Config';  // Đảm bảo đường dẫn đúng đến tệp config

// Tạo một instance của axios với baseURL
const api = axios.create({
  baseURL: config.apiBaseUrl,  // Sử dụng apiBaseUrl từ config.js
  headers: {
    'Content-Type': 'application/json',  // Content-Type là json
  },
});

// Hàm lấy token từ localStorage
const getAuthToken = () => {
  return localStorage.getItem('authToken');  // Lấy token từ localStorage
};

// API đăng nhập (đã có trong code của bạn)
export const loginAPI = (UserName, Password) => {
  return api.post('/SysUsers/Login', { UserName, Password });
};

// API đăng ký
export const registerAPI = (FullName, UserName, Password, ConfirmPassword, Email, Status, Note) => {
  return api.post('/SysUsers/Register', { FullName, UserName, Password, ConfirmPassword, Email, Status, Note });
};

// API lấy danh sách người dùng
export const getUsersAPI = () => {
  return api.get('/SysUsers/UsersList', {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,  // Thêm token vào header
    },
  });
};


// API thêm người dùng
export const addUserAPI = (userData) => {
  return api.post('/SysUsers/Add User', userData, {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,  // Thêm token vào header
    },
  });
};

// API cập nhật người dùng
export const updateUserAPI = (userData) => {
  return api.post(`/SysUsers/Update User`, userData, {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,  // Thêm token vào header
    },
  });
};

// API xóa người dùng
export const deleteUserAPI = (userId) => {
  const token = localStorage.getItem('authToken'); // Lấy token từ localStorage
  return api.post(`/SysUsers/Delete%20User?userId=${userId}`, null, {
    headers: {
      Authorization: `Bearer ${token}`, // Thêm token vào tiêu đề
    },
  });
};

export const getBooksAPI = () => {
  return api.get('/Books/BooksList', {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,
    },
  });
};

export const addBookAPI = (bookData) => {
  return api.post('/Books/Add Book',bookData, {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,
    },  
  });
};

export const updateBookAPI = (bookData) => {
  return api.post('/Books/Update Book',bookData, {
    headers: {
      'Authorization': `Bearer ${getAuthToken()}`,
    },
  });
};

export const deleteBookAPI = (bookId) => {
  const token = localStorage.getItem('authToken'); // Lấy token từ localStorage
  return api.post(`/Books/Delete%20Book?bookId=${bookId}`, null, {
    headers: {
      Authorization: `Bearer ${token}`, // Thêm token vào tiêu đề
    },
  });
};
