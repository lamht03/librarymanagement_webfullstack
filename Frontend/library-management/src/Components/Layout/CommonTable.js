import React, { useState } from "react";
import { Table, Button, Input, Pagination } from "antd";
import { EditOutlined, DeleteOutlined, SearchOutlined, PlusOutlined } from "@ant-design/icons";
import "../../Assets/Styles/Layout/CommonTable.css";

const CommonTable = ({ columns, data, onEdit, onDelete, onSearch, title, showModal }) => {
  const [currentPage, setCurrentPage] = useState(1);
  const pageSize = 5; // Số bản ghi mỗi trang

  const handleSearch = (e) => {
    if (onSearch) onSearch(e.target.value);
  };

  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const extendedColumns = [
    ...columns,
    {
      title: "Actions",
      key: "actions",
      render: (_, record) => (
        <div className="action-buttons">
          <Button
            icon={<EditOutlined />}
            onClick={() => onEdit(record)}
            className="action-button edit-btn"
          />
          <Button
            icon={<DeleteOutlined />}
            onClick={() => onDelete(record)}
            className="action-button delete-btn"
          />
        </div>
      ),
    },
  ];

  return (
    <div className="common-table-container">
      <div className="table-header">
        <h2 className="table-title">{title}</h2>
        <Input
          placeholder="Search..."
          onChange={handleSearch}
          prefix={<SearchOutlined />}
          className="search-input"
        />
      </div>

      <div className="table-container">
        <Button
          type="primary"
          icon={<PlusOutlined />}
          onClick={() => showModal(null)}
          className="add-btn"
        />

        <Table
          columns={extendedColumns}
          dataSource={data}
          pagination={{
            current: currentPage,
            pageSize: pageSize,
            total: data.length,
            onChange: handlePageChange,
          }}
          rowKey="BookID"
          bordered
          className="styled-table"
        />
      </div>
    </div>
  );
};

export default CommonTable;