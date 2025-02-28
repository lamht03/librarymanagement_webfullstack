import React from "react";
import "../../Assets/Styles/Authentication/HeaderTitle.css"

const HeaderTitle = ({ title }) => {
  return (
    <div className="Header-Title">
      {title}
      <i className="fa-solid fa-pen icon-pen" style={{ color: "#FCFF7D", marginRight: "8px", fontSize: "25px" }}></i>
      <i className="fas fa-thin fa-book-bookmark icon-book" style={{ color: "#33FFCC", marginRight: "8px", fontSize: "30px" }}></i>
    </div>
  );
};

export default HeaderTitle;
