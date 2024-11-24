import React from "react";

const Card = ({
  row,
  columns,
  editMode,
  setEditMode,
  handleUpdateRow,
  handleDeleteRow,
  renderCellContent,
  handleCancelEdit
}) => {
  return (
    <div className="card mb-3 shadow-sm">
      <div className="card-body">
        <h5 className="card-title">
          {row.name || `Card #${row.id}`}
        </h5>
        <h6 className="card-subtitle mb-2 text-muted">
          ID: {row.id}
        </h6>

        <div className="row">
          {columns.map((column, colIndex) => (
            <div
              key={`${row.id}-${column.field}`}
              className="col-12 col-md-6 mb-2"
            >
              <strong>{column.headerName}:</strong>{" "}
              {renderCellContent(row, column, colIndex)}
            </div>
          ))}
        </div>

        <div className="mt-3 d-flex justify-content-end gap-2">
          {editMode === row.id ? (
            <div>
              <button
              className="btn btn-success btn-sm"
              onClick={() => handleUpdateRow(row.id, row)}>
              Update
              </button>
              <button className="btn btn-secondary btn-sm" onClick={handleCancelEdit}>
                Cancel
              </button>
            </div>
          ) : (
            <button
              className="btn btn-warning btn-sm"
              onClick={() => setEditMode(row.id)}
            >
              Edit
            </button>
          )}
          <button
            className="btn btn-danger btn-sm"
            onClick={() => handleDeleteRow(row.id)}
          >
            Delete
          </button>
        </div>
      </div>
    </div>
  );
};

export default Card;
