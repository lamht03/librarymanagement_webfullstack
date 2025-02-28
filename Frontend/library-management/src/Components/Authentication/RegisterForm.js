import React, { useState } from "react";
import { Form, Input, Button } from "antd";
import { EyeInvisibleOutlined, EyeTwoTone } from '@ant-design/icons';
import HeaderTitle from "./HeaderTitle";
import Logo from "../../Assets/Images/Img_Logo_LoginForm_Tdu.png"
import "../../Assets/Styles/Authentication/RegisterForm.css"
import { registerAPI } from "../../Utilities/Api";
import { message } from "antd";


const handleRegister = async (values, handleSwitchMode) => {
  try {
    // Gọi API đăng ký
    const response = await registerAPI("",values.Username,values.Password,values.ConfirmPassword,values.Email,true,"Regular user");

    if (response.data.Status === 1) {
      message.success("Registration successful!");
      handleSwitchMode();  // Chuyển sang màn hình đăng nhập
    } else if (response.data.Status === 0) {
      message.error(response.data.Message || "Registration failed. Please try again.");
    }
  }  catch (error) {
    console.error("Login API error:", error);
    
    // Hiển thị thông báo lỗi nếu có lỗi từ API
    if (error.response) {
      // Lỗi từ API trả về
      message.error(error.response.data.Message || "An error occurred during login. Please try again.");
    } else {
      // Lỗi khi gọi API (network, CORS, v.v.)
      message.error("An error occurred while connecting to the server. Please try again.");
    }
  }
};

const FormRegister = ({ onFinish, handleSwitchMode }) => {
  return (
    <Form
      name="Register"
      className="Register-Form"
      onFinish={(values) => handleRegister(values, handleSwitchMode)}
    >
      <img src={Logo} alt="Thanh Do University Logo" className="Login-Logo" />
      <HeaderTitle title="Register for TDU Library" />
      <Form.Item name="Email" rules={[{ required: true, message: "Enter your Email!" }]}>
        <Input placeholder="Email" className="Styled-Input" />
      </Form.Item>
      <Form.Item name="Username" rules={[{ required: true, message: "Enter your UserName!" }]}>
        <Input placeholder="UserName" className="Styled-Input" />
      </Form.Item>
      <Form.Item name="Password" rules={[{ required: true, message: "Enter your Password!" }]}>
        <Input.Password
          placeholder="Password"
          className="Styled-Input"
          iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
          visibilityToggle
        />
      </Form.Item>
      <Form.Item name="ConfirmPassword" rules={[{ required: true, message: "Confirm your Password!" }]}>
        <Input.Password
          placeholder="Confirm Password"
          className="Styled-Input"
          iconRender={visible => (visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />)}
          visibilityToggle
        />
      </Form.Item>
      <Form.Item>
        <Button type="primary" htmlType="submit" block className="Register-Button">
          REGISTER
        </Button>
      </Form.Item>
      <div className="Switch-Mode">
        Already have an account? <span onClick={handleSwitchMode}>Sign In</span>
      </div>
    </Form>
  );
};

export default FormRegister;
