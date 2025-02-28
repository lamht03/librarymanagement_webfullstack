import React, { useState } from "react";
import { message } from "antd";
import axios from "axios";
import "../Assets/Styles/Pages/Authentication.css";
import FormLogin from "../Components/Authentication/LoginForm";
import FormRegister from "../Components/Authentication/RegisterForm";
import ImageSide from "../Components/Authentication/ImageSide";
import "../Assets/Styles/Authentication/Global.css"


const Login = () => {
  const [loading, setLoading] = useState(false);
  const [isRegister, setIsRegister] = useState(false);
  const [fadeOut, setFadeOut] = useState(false);

  const onFinish = async (values) => {
    setLoading(true);
    try {
      const response = await axios.post("http://localhost:5000/api/auth/login", values);
      localStorage.setItem("token", response.data.token);
      message.success("Welcome to ThanhDo Library! You’re now logged in.");
    } catch (error) {
      message.error("Invalid username or password.");
    } finally {
      setLoading(false);
    }
  };

  const handleSwitchMode = () => {
    setFadeOut(true);
    setTimeout(() => {
      setIsRegister(!isRegister);
      setFadeOut(false);
    }, 50);
  };

  return (
    <div className="Login-Container">
      {!fadeOut && (
        <div className={`Login-Wrapper ${fadeOut ? "fade-out" : "fade-in"}`}>
          <div className="Login-Form-Side">
            {isRegister ? (
              <FormRegister onFinish={onFinish} handleSwitchMode={handleSwitchMode} loading = {loading} />
            ) : (
              <FormLogin onFinish={onFinish} handleSwitchMode={handleSwitchMode} loading={loading}/>
            )}
          </div>
          <ImageSide></ImageSide>
        </div>
      )}
    </div>
  );
};

export default Login;
