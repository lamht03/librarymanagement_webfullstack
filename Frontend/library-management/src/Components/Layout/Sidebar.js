import React, { useState } from 'react';
import { Link } from 'react-router-dom';
import '../../Assets/Styles/Layout/Sidebar.css';

const Sidebar = () => {
  const [isClosed, setIsClosed] = useState(false);

  const toggleSidebar = () => {
    setIsClosed(!isClosed);
  };

  return (
    <div className={`Sidebar ${isClosed ? 'Closed' : ''}`}>
      <button className="Toggle-Btn" onClick={toggleSidebar}>
        {isClosed ? '☰' : '×'}
      </button>
      <nav>
        <ul>
          <li>
            <i className="fas fa-users"></i>
            <Link to="/user-management">User Management</Link>
          </li>
          <li>
            <i className="fas fa-book"></i>
            <Link to="/book-management">Book Management</Link>
          </li>
          <li>
            <i className="fas fa-credit-card"></i>
            <a href="#Transaction-Management">Transaction</a>
          </li>
          <li>
            <i className="fas fa-dollar-sign"></i>
            <a href="#Payment-Management">Payment</a>
          </li>
        </ul>
      </nav>
    </div>
  );
};

export default Sidebar;


