import React, { useEffect } from 'react';
import { Modal, Form, Input, message } from 'antd';

const UserModal = ({ isModalVisible, setIsModalVisible, currentUser, isEdit, setData }) => {
  const [form] = Form.useForm();

  useEffect(() => {
    if (currentUser) {
      form.setFieldsValue(currentUser);
    } else {
      form.resetFields();
    }
  }, [currentUser, form]);

  const handleOk = () => {
    form
      .validateFields()
      .then((values) => {
        if (isEdit) {
          // Edit user
          setData(prevData => prevData.map(item => 
            item.UserID === currentUser.UserID ? { ...item, ...values } : item
          ));
          message.success('User updated successfully');
        } else {
          // Add new user
          setData(prevData => [...prevData, { ...values, UserID: Date.now() }]);
          message.success('User added successfully');
        }
        setIsModalVisible(false);
      })
      .catch((error) => {
        console.log('Validation failed:', error);
      });
  };

  const handleCancel = () => {
    setIsModalVisible(false);
  };

  return (
    <Modal
      title={isEdit ? 'Edit User' : 'Add User'}
      open={isModalVisible}
      onOk={handleOk}
      onCancel={handleCancel}
      okText={isEdit ? 'Update' : 'Add'}
      cancelText="Cancel"
    >
      <Form form={form} layout="vertical">
        <Form.Item
          label="Full Name"
          name="FullName"
          rules={[{ required: true, message: 'Please input full name!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="UserName"
          name="UserName"
          rules={[{ required: true, message: 'Please input user name!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Email"
          name="Email"
          rules={[{ required: true, message: 'Please input email!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Password"
          name="Password"
          rules={[{ required: true, message: 'Please input password!' }]}
        >
          <Input.Password />
        </Form.Item>
        <Form.Item
          label="Note"
          name="Note"
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default UserModal;
