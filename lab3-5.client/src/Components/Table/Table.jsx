import React, { useState, useEffect } from "react";
import axios from "axios";
import Card from '../Cards/PersonCard'
import DeliveryForm from "../Forms/DeliveryForm";

const Table = ({ columns, initialRows, entityType }) => {
  const [rows, setRows] = useState(initialRows);
  const [newRow, setNewRow] = useState(
    columns.reduce((acc, column) => {
      acc[column.field] = "";
      return acc;
    }, {})
  );
  const [expandedRow, setExpandedRow] = useState(null);
  const [editMode, setEditMode] = useState(null); // State to track the edit mode
  const [showModal, setShowModal] = useState(false); // State to control modal visibility

  useEffect(() => {
    setRows(initialRows);
  }, [initialRows]);

  const handleUpdateRow = (id, updatedRow) => {
    console.log(id, updatedRow);
    axios
      .put(`/api/${entityType}/${id}`, updatedRow)
      .then((response) => {
        // Переконаємося, що response.data — об'єкт
        console.log(response)
        if (typeof response.data !== "object" || response.data === null) {
          throw new Error("Invalid response data format");
        }
  
        // Додавання id залежно від типу сутності
        if (entityType === "Person") {
          response.data.id = response.data.personId;
        } else if (entityType === "Delivery") {
          response.data.id = response.data.deliveryId;
        }
  
        console.log("Row updated successfully:", response.data);
  
        // Оновлення рядка у стані
        setRows((prevRows) =>
          prevRows.map((row) => (row.id === id ? response.data : row))
        );
        setEditMode(null); // Вихід із режиму редагування
      })
      .catch((error) => {
        console.error("Error updating row:", error);
        alert("Failed to update the row. Please try again.");
      });
  };
  

  const handleEdit = (id, field, value) => {
    setRows((prevRows) =>
      prevRows.map((row) =>
        row.id === id ? { ...row, [field]: value } : row
      )
    );
  };
  const handleCancelEdit = () => {
    setEditMode(null);
  };

  const handleAddRow = () => {
    //if (Object.values(newRow).every((value) => value.trim())) {
      console.log(newRow);
      axios
        .post(`/api/${entityType}`, newRow)
        .then((response) => {
          console.log(response);
          setRows((prevRows) => [{ ...response.data }, ...prevRows]);
          setNewRow(
            columns.reduce((acc, column) => {
              acc[column.field] = "";
              return acc;
            }, {})
          );
          setShowModal(false);
        })
        .catch(error => {
          console.log(error);
          if (error.response && error.response.status === 400) {
              // Îòðèìóºìî ñïèñîê ïîìèëîê ç â³äïîâ³ä³ ñåðâåðà
              const errorMessages = error.response.data.errors;
      
              // Ôîðìóºìî îäíå ïîâ³äîìëåííÿ ç óñ³õ ïîìèëîê
              let fullErrorMessage = '';
              for (const field in errorMessages) {
                  if (errorMessages.hasOwnProperty(field)) {
                      fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                  }
              }
      
              // Âèâîäèìî ïîâ³äîìëåííÿ êîðèñòóâà÷ó (íàïðèêëàä, ÷åðåç alert àáî ó âàø UI)
              alert(error.response.data.message);
          }
          if (error.response && error.response.status === 401) {
              alert(error.response.data.message + error.response.data.details);
          }
          //onClose();
      });
    // } else {
    //   alert("Please fill in all fields before adding.");
    // }
  };
  

  const handleDeleteRow = (id) => {
    axios
      .delete(`/api/${entityType}/${id}`)
      .then(() => {
        setRows((prevRows) => prevRows.filter((row) => row.id !== id));
      })
      .catch((error) => {
        console.error("Error deleting row:", error);
        alert("Failed to delete the row. Please try again.");
      });
  };

  const toggleExpandRow = (id) => {
    setExpandedRow((prev) => (prev === id ? null : id));
  };

  const renderCellContent = (row, column, colIndex) => {
    const value = row[column.field];
  
    if (value && typeof value === "object") {
      return (
        <>
          <button
            className="btn btn-info btn-sm"
            onClick={() => toggleExpandRow(row.id)}
          >
            {expandedRow === row.id ? "Hide Details" : "Show Details"}
          </button>
          {expandedRow === row.id && (
            <div className="mt-2 p-2 border">
              {Object.entries(value).map(([key, val]) => (
                <div key={key}>
                  <strong>{key}:</strong> {val || "N/A"}
                </div>
              ))}
            </div>
          )}
        </>
      );
    }
  
    if (editMode === row.id) {
      if (column.field === "personId" || column.field === "deliveryId" || column.field === "courier" || column.field === "client" || column.field === "address" || column.field === "order" || column.field === "warehouse") {
        return <span>{value || "N/A"}</span>;
      }
      if (column.field === "status") {
        return (
          <select
            value={value || ""}
            onChange={(e) => handleEdit(row.id, column.field, e.target.value)}
            className="form-control"
          >
            <option value="pending">Pending</option>
            <option value="in process">In Process</option>
            <option value="completed">Completed</option>
          </select>
        );
      }
      return (
        <input
          type="text"
          value={value || ""}
          onChange={(e) => handleEdit(row.id, column.field, e.target.value)}
          className="form-control"
        />
      );
    }
  
    return <span>{value || "N/A"}</span>;
  };
  

  const sortedRows = [...rows].sort((a, b) => b.id - a.id);

  return (
    <div className="container mt-4">
      <button className="btn btn-primary mb-3" onClick={() => setShowModal(true)}>
        Add New Row
      </button>

      {/* Modal for adding new row
      {showModal && (
        <div className="modal show d-block" style={{ backgroundColor: "rgba(0, 0, 0, 0.5)" }}>
          <div className="modal-dialog">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">Add New {entityType}</h5>
                <button type="button" className="close" onClick={() => setShowModal(false)}>
                  <span>&times;</span>
                </button>
              </div>
              <div className="modal-body">
                {columns.map((column, index) => (
                  index === 0 ? null : (
                    <div key={`new-${column.field}`} className="form-group">
                      <input
                        type="text"
                        value={newRow[column.field] || ""}
                        onChange={(e) =>
                          setNewRow((prev) => ({
                            ...prev,
                            [column.field]: e.target.value,
                          }))
                        }
                        placeholder={`Enter ${column.headerName}`}
                        className="form-control"
                      />
                    </div>
                  )
                ))}
              </div>
              <div className="modal-footer">
                <button className="btn btn-secondary" onClick={() => setShowModal(false)}>
                  Close
                </button>
                <button className="btn btn-primary" onClick={handleAddRow}>
                  Add Row
                </button>
              </div>
            </div>
          </div>
        </div>
      )} */}
      {/* Rows display */}
      {sortedRows.map((row) => (
        <Card
        key={row.id}
        row={row}
        columns={columns}
        handleUpdateRow={handleUpdateRow}
        handleDeleteRow={handleDeleteRow}
        renderCellContent={renderCellContent}
        handleCancelEdit={handleCancelEdit}
        />
      ))}
    </div>
  );
};

export default Table;
