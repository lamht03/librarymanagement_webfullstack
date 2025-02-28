import React from "react";
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from "./Pages/Authentication";
import UserManagement from "./Pages/UserManagement"; // Trang quản lý người dùng
import ProtectedRoute from "./Utilities/ProtectedRoute"; // Add this import
import BookManagement from './Pages/BookManagement';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <Router>
          <Routes>
            <Route path="" element={<Login />} />
            {/* Protected route cho trang quản lý người dùng */}
            <Route
              path="/user-management"
              element={
                <ProtectedRoute>
                  <UserManagement />
                </ProtectedRoute>
              }
            />
            <Route
              path="/book-management"
              element={
                <ProtectedRoute>
                  <BookManagement />
                </ProtectedRoute>
              }
            />
          </Routes>
        </Router>
      </header>
    </div>
  );
}

export default App;
