import React from "react";
import { Typography } from "antd";
import "../../Assets/Styles/Authentication/QuoteSection.css"



const { Text } = Typography;

const Quote = () => {
  return (
    <div className="Quote-Container">
      <Text className="Quote-Text">
        "It is from books that wise people derive consolation in the troubles of life."
      </Text>
      <Text className="Quote-Author">- Victor Hugo</Text>
    </div>
  );
};

export default Quote;
