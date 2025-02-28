import React, { useState, useEffect } from 'react';
import { Modal, Form, message } from 'antd';
import CommonTable from '../CommonTable';
import CommonModal from '../../Modal/CommonModal';
import { getBooksAPI, updateBookAPI, deleteBookAPI,addBookAPI } from '../../../Utilities/Api';

const BookContent = () => {
  const [books, setBooks] = useState([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [form] = Form.useForm();
  const [currentRecord, setCurrentRecord] = useState(null);
  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [deleteContent, setDeleteContent] = useState('');

  useEffect(() => {
    const fetchBooks = async () => {
      try {
        const response = await getBooksAPI();
        console.log('API Response:', response.data.Data);
        setBooks(response.data.Data);
      } catch (error) {
        message.error('Error fetching books');
      }
    };
    fetchBooks();
  }, []);

  const columns = [
    { title: 'Title', dataIndex: 'Title', key: 'Title' },
    { title: 'Author', dataIndex: 'Author', key: 'Author' },
    { title: 'Genre', dataIndex: 'Genre', key: 'Genre' },
    { title: 'Published Date', dataIndex: 'PublishedDate', key: 'PublishedDate' },
    { title: 'Total Quantity', dataIndex: 'TotalQuantity', key: 'TotalQuantity' },
    { title: 'Available Quantity', dataIndex: 'AvailableQuantity', key: 'AvailableQuantity' },
  ];

  const handleAdd = () => {
    setIsEdit(false);
    form.resetFields();
    setIsModalVisible(true);
  };

  const handleEdit = (record) => {
    setIsEdit(true);
    form.setFieldsValue(record);
    setCurrentRecord(record);
    setIsModalVisible(true);
  };

  const handleDelete = (record) => {
    setDeleteContent(`You are about to delete the record of ${record.Title}. This action cannot be undone.`);
    setIsDeleteModalVisible(true);
    setCurrentRecord(record);
  };

  const handleDeleteConfirm = async () => {
    try {
      const response = await deleteBookAPI(currentRecord.BookID);
      setBooks(books.filter(book => book.BookID !== currentRecord.BookID));
      setIsDeleteModalVisible(false);
      message.success(response.data.Message || 'Book deleted successfully');
    } catch (error) {
      message.error(error.response?.data?.Message || 'Error deleting Book');
    }
  };

  const handleDeleteCancel = () => {
    setIsDeleteModalVisible(false);
  };

  const handleCancel = () => setIsModalVisible(false);

  const handleSubmit = async () => {
    form.validateFields().then(async (values) => {
      if (isEdit) {
        // Update book logic here
        try {
          const response = await updateBookAPI({ ...values, BookID: currentRecord.BookID });          
          setBooks(books.map(book => book.BookID === currentRecord.BookID ? { ...book, ...values } : book));
          setIsModalVisible(false);
          message.success(response.data.Message || 'Book updated successfully');
        } catch (error) {
          message.error(error.response?.data?.Message || 'Error updating Book');
        }
      } else {
        // Add book logic here
        try {
          const response = await addBookAPI(values);
          setBooks([...books, values]);
          setIsModalVisible(false);
          message.success(response.data.Message || 'Book added successfully');
        } catch (error) {
          message.error(error.response?.data?.Message || 'Error adding Book');
        }
      }
    });
  };

  return (
    <>
      <CommonTable
        columns={columns}
        data={books}
        title="Book Management"
        onEdit={handleEdit}
        onDelete={handleDelete}
        onSearch={() => {}}
        pagination={true}
        showModal={handleAdd}
      />
      <CommonModal
        isModalVisible={isModalVisible}
        handleCancel={handleCancel}
        handleSubmit={handleSubmit}
        isEdit={isEdit}
        form={form}
        columns={columns}
        isDeleteModalVisible={isDeleteModalVisible}
        handleDeleteCancel={handleDeleteCancel}
        handleDeleteConfirm={handleDeleteConfirm}
        deleteContent={deleteContent}
      />
    </>
  );
};

export default BookContent; 