import React, { useState } from "react";
import { Form, Input, Button, message } from "antd";
import HeaderTitle from "./HeaderTitle";
import ForgotPasswordModal from "../Modal/ForgotPasswordModal"; // Import modal
import Logo from "../../Assets/Images/Img_Logo_LoginForm_Tdu.png";
import QuoteSection from "./QuoteSection";
import "../../Assets/Styles/Authentication/LoginForm.css";
import { useNavigate } from "react-router-dom"; // Add this import
import { EyeInvisibleOutlined, EyeTwoTone } from "@ant-design/icons";
import "../../Utilities/Config";
import { loginAPI } from "../../Utilities/Api"; // Add this import

const FormLogin = ({ loading, handleSwitchMode }) => {
  const [isForgotPasswordOpen, setForgotPasswordOpen] = useState(false);
  const [passwordVisible, setPasswordVisible] = useState(false); // State for password visibility
  const navigate = useNavigate(); // Hook điều hướng của react-router-dom

  // Mở modal Forgot Password
  const handleOpenForgotPassword = () => setForgotPasswordOpen(true);
  // Đóng modal Forgot Password
  const handleCloseForgotPassword = () => setForgotPasswordOpen(false);

  // Hàm xử lý khi người dùng nhấn đăng nhập
  // Xử lý khi người dùng đăng nhập
  const handleLoginSuccess = async (values) => {
    try {
      // Gọi API đăng nhập
      const response = await loginAPI(values.UserName, values.Password);
      
      // Kiểm tra nếu response có token và không có lỗi
      if (response.data.token) {
        // Lưu token và refreshToken vào localStorage
        localStorage.setItem("isAuthenticated", "true");  // Lưu trạng thái đăng nhập
        localStorage.setItem("authToken", response.data.token);  // Lưu token
        localStorage.setItem("refreshToken", response.data.refreshToken);  // Lưu refreshToken
  
        // Điều hướng đến trang quản lý người dùng
        navigate("/user-management");  
        message.success("Login successful!");
      } else {
        // Nếu có thông báo lỗi từ backend, hiển thị thông báo đó
        message.error(response.data.message || "An error occurred during login. Please try again.");
      }
    } catch (error) {
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
  
  return (
    <>
      <Form name="Login" onFinish={handleLoginSuccess} className="Login-Form">
        <img src={Logo} alt="Thanh Do University Logo" className="Login-Logo" />
        <HeaderTitle title="Welcome to TDU Library" />
        <Form.Item
          name="UserName"
          rules={[{ required: true, message: "Enter your UserName!" }]}
        >
          <Input placeholder="UserName" className="Styled-Input" />
        </Form.Item>
        <Form.Item
          name="Password"
          rules={[{ required: true, message: "Enter your Password!" }]}
        >
          <Input.Password
            placeholder="Password"
            className="Styled-Input"
            iconRender={(visible) =>
              visible ? <EyeTwoTone /> : <EyeInvisibleOutlined />
            }
            visibilityToggle
          />
        </Form.Item>
        <Form.Item className="custom-form-item">
          <Button
            type="primary"
            htmlType="submit"
            loading={loading}
            block
            className="Login-Button"
          >
            LOG IN
          </Button>
        </Form.Item>
        <div className="Login-Footer">
          <a
            onClick={handleOpenForgotPassword}
            className="Forgot-Password-Link"
          >
            Forgot Password?
          </a>

          {/* Gọi modal quên mật khẩu */}
          <ForgotPasswordModal
            isOpen={isForgotPasswordOpen}
            onClose={handleCloseForgotPassword}
          />
        </div>
        <QuoteSection />
        <div className="Switch-Mode">
          Don’t have an account? <span onClick={handleSwitchMode}>Sign Up</span>
        </div>
      </Form>
    </>
  );
};

export default FormLogin;
