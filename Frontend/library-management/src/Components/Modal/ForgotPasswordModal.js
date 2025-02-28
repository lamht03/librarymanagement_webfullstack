import React, { useState } from "react";
import { Modal, Input, Button, message } from "antd";
import axios from "axios";
import "../../Assets/Styles/Modal/ForgotPasswordModal.css"; // CSS mới cho modal


const ForgotPasswordModal = ({ isOpen, onClose }) => {
  const [email, setEmail] = useState("");
  const [loading, setLoading] = useState(false);

  const handleForgotPassword = async () => {
    if (!email) {
      message.error("Please enter your email.");
      return;
    }

    setLoading(true);
    try {
      // Gửi yêu cầu quên mật khẩu
      await axios.post("http://localhost:5000/api/auth/forgot-password", { email });
      message.success("A reset link has been sent to your email. Please check.");
      onClose(); // Đóng modal khi xử lý thành công
    } catch (error) {
      message.error("An error occurred. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal
      title="Forgot Password"
      open={isOpen}
      onCancel={onClose}
      footer={[
        <Button
          key="cancel"
          onClick={onClose}
          className="forgot-password-cancel-button"
        >
          Cancel
        </Button>,
        <Button
          key="submit"
          type="primary"
          onClick={handleForgotPassword}
          className="forgot-password-submit-button"
          loading={loading}
        >
          Submit
        </Button>,
      ]}
      className="forgot-password-modal"
    >
      <p className="forgot-password-text">
        Please enter your email to receive a new password:
      </p>
      <Input
        placeholder="Enter your email"
        value={email}
        onChange={(e) => setEmail(e.target.value)}
        className="forgot-password-input"
      />
    </Modal>
  );
};

export default ForgotPasswordModal;
