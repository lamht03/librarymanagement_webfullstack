import React from 'react';
import { Modal, Form, Input } from 'antd';
import "../../Assets/Styles/Modal/CommonModal.css";

const CommonModal = ({ 
  isModalVisible, 
  handleCancel, 
  handleSubmit, 
  isEdit, 
  form, 
  columns, 
  isDeleteModalVisible, 
  handleDeleteCancel, 
  handleDeleteConfirm, 
  deleteContent 
}) => {
  return (
    <>
      <Modal
        title={isEdit ? "Edit Record" : "Add Record"}
        open={isModalVisible}
        onCancel={handleCancel}
        onOk={handleSubmit}
        className="custom-modal"
      >
        <Form form={form} layout="vertical">
          {columns.map((col) => {
            if (col.dataIndex === "actions") return null;
            return (
              <Form.Item
                key={col.dataIndex}
                label={col.title}
                name={col.dataIndex}
                rules={[
                  { required: true, message: `Please enter ${col.title}` },
                ]}
              >
                <Input />
              </Form.Item>
            );
          })}
        </Form>
      </Modal>

      <Modal
        title="Delete Confirmation"
        open={isDeleteModalVisible}
        onCancel={handleDeleteCancel}
        onOk={handleDeleteConfirm}
        className="delete-modal"
      >
        <p>{deleteContent}</p>
      </Modal>
    </>
  );
};

export default CommonModal; 