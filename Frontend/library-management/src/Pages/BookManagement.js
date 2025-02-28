import React from 'react';
import Header from '../Components/Layout/Header';
import Footer from '../Components/Layout/Footer';
import Sidebar from '../Components/Layout/Sidebar';
import BookContent from '../Components/Layout/Content/BookContent';

const BookManagement = () => {
  return (
    <div className="book-management">
      <Header />
      <div className="main-content">
        <Sidebar />
        <div className="content-area">
          <BookContent />
        </div>
      </div>
      <Footer />
    </div>
  );
};

export default BookManagement; 