import React, { useState, useEffect } from 'react';
import { Modal, Form, message } from 'antd';
import CommonTable from '../CommonTable';
import CommonModal from '../../Modal/CommonModal';
import { getUsersAPI, deleteUserAPI, updateUserAPI, addUserAPI } from '../../../Utilities/Api';

const UserContent = () => {
  const [users, setUsers] = useState([]);
  const [isModalVisible, setIsModalVisible] = useState(false);
  const [isEdit, setIsEdit] = useState(false);
  const [form] = Form.useForm();
  const [currentRecord, setCurrentRecord] = useState(null);
  const [isDeleteModalVisible, setIsDeleteModalVisible] = useState(false);
  const [deleteContent, setDeleteContent] = useState('');

  useEffect(() => {
    const fetchUsers = async () => {
      try {
        const response = await getUsersAPI();
        setUsers(response.data.Data);
      } catch (error) {
        message.error(error.response?.data?.Message || 'Error fetching users');
      }
    };
    fetchUsers();
  }, []);

  const columns = [
    { title: 'Full Name', dataIndex: 'FullName', key: 'FullName' },
    { title: 'Username', dataIndex: 'UserName', key: 'UserName' },
    { title: 'Email', dataIndex: 'Email', key: 'Email' },
    { title: 'Password', dataIndex: 'Password', key: 'Password' },
    { title: 'Note', dataIndex: 'Note', key: 'Note' },
  ];

  const handleAdd = () => {
    setIsEdit(false);
    setCurrentRecord(null);
    setIsModalVisible(true);
  };

  const handleEdit = (record) => {
    setIsEdit(true);
    form.setFieldsValue(record);
    setCurrentRecord(record);
    setIsModalVisible(true);
  };

  const handleSearch = (searchTerm) => {
    setUsers(users.filter(user => user.fullName.toLowerCase().includes(searchTerm.toLowerCase())));
  };

  const handleDelete = (record) => {
    setDeleteContent(`You are about to delete the record of ${record.FullName}. This action cannot be undone.`);
    setIsDeleteModalVisible(true);
    setCurrentRecord(record);
  };

  const handleDeleteConfirm = async () => {
    try {
      const response = await deleteUserAPI(currentRecord.UserID);
      setUsers(users.filter(user => user.UserID !== currentRecord.UserID));
      setIsDeleteModalVisible(false);
      message.success(response.data.Message || 'User deleted successfully');
    } catch (error) {
      message.error(error.response?.data?.Message || 'Error deleting user');
    }
  };

  const handleDeleteCancel = () => {
    setIsDeleteModalVisible(false);
  };

  const handleCancel = () => setIsModalVisible(false);

  const handleSubmit = async () => {
    form.validateFields().then(async (values) => {
      if (isEdit) {
        try {
          const response = await updateUserAPI({ ...values, UserID: currentRecord.UserID });
          setUsers(users.map(user => user.UserID === currentRecord.UserID ? { ...user, ...values } : user));
          setIsModalVisible(false);
          message.success(response.data.Message || 'User updated successfully');
        } catch (error) {
          message.error(error.response?.data?.Message || 'Error updating user');
        }
      } else {
        try {
          const response = await addUserAPI(values);
          setUsers([...users, response.data]);
          setIsModalVisible(false);
          message.success(response.data.Message || 'User added successfully');
        } catch (error) {
          message.error(error.response?.data?.Message || 'Error adding user');
        }
      }
    });
  };

  return (
    <>
      <CommonTable
        columns={columns}
        data={users}
        title="User Management"
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

export default UserContent;
