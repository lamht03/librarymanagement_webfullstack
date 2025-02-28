import React, { useEffect } from 'react';
import { Modal, Form, Input, DatePicker, InputNumber, message } from 'antd';
import moment from 'moment';

const BookModal = ({ isModalVisible, setIsModalVisible, currentBook, isEdit, setData }) => {
  const [form] = Form.useForm();

  useEffect(() => {
    if (currentBook) {
      form.setFieldsValue({
        ...currentBook,
        PublishedDate: currentBook.PublishedDate ? moment(currentBook.PublishedDate) : null,
      });
    } else {
      form.resetFields();
    }
  }, [currentBook, form]);

  const handleOk = () => {
    form
      .validateFields()
      .then((values) => {
        if (isEdit) {
          // Edit book
          setData(prevData => prevData.map(item => 
            item.BookID === currentBook.BookID ? { ...item, ...values } : item
          ));
          message.success('Book updated successfully');
        } else {
          // Add new book
          setData(prevData => [...prevData, { ...values, BookID: Date.now() }]);
          message.success('Book added successfully');
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
      title={isEdit ? 'Edit Book' : 'Add Book'}
      open={isModalVisible}
      onOk={handleOk}
      onCancel={handleCancel}
      okText={isEdit ? 'Update' : 'Add'}
      cancelText="Cancel"
    >
      <Form form={form} layout="vertical">
        <Form.Item
          label="Title"
          name="Title"
          rules={[{ required: true, message: 'Please input book title!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Author"
          name="Author"
          rules={[{ required: true, message: 'Please input author name!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Genre"
          name="Genre"
          rules={[{ required: true, message: 'Please input genre!' }]}
        >
          <Input />
        </Form.Item>
        <Form.Item
          label="Published Date"
          name="PublishedDate"
          rules={[{ required: true, message: 'Please select published date!' }]}
        >
          <DatePicker />
        </Form.Item>
        <Form.Item
          label="Total Quantity"
          name="TotalQuantity"
          rules={[{ required: true, message: 'Please input total quantity!' }]}
        >
          <InputNumber min={0} />
        </Form.Item>
        <Form.Item
          label="Description"
          name="Description"
        >
          <Input />
        </Form.Item>
      </Form>
    </Modal>
  );
};

export default BookModal; 