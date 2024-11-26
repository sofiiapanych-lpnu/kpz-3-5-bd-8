import { useState, useEffect } from "react";
import axios from 'axios';
import DeliveryContainer from "../../Components/CardsContainers/DeliveryContainer";
import DeliveryForm from "../../Components/Forms/DeliveryForm";
import './Delivery.css';

const Delivery = () => {
  const [delivery, setDelivery] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  const [status, setStatus] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [isFormOpen, setIsFormOpen] = useState(false);
  const [activeStatus, setActiveStatus] = useState("");

  const fetchDelivery = () => {
    setIsLoading(true);
    let queryParams = "";

    if (status) queryParams += `?status=${status}`;
    if (startDate) queryParams += `${queryParams ? "&" : "?"}startDate=${startDate}`;
    if (endDate) queryParams += `${queryParams ? "&" : "?"}endDate=${endDate}`;

    axios.get(`api/Delivery${queryParams}`)
      .then(response => {
        setDelivery(response.data);
        setIsLoading(false);
      })
      .catch(() => {
        setError('Error fetching deliveries.');
        setIsLoading(false);
      });
  };

  const assignCouriers = () => {
    axios.post("api/Delivery/assign-couriers")
      .then(response => {
        fetchDelivery();
        alert(response.data.message);
      })
      .catch(() => {
        alert("An error occurred while assigning couriers.");
      });
  };

  useEffect(() => {
    fetchDelivery();
  }, [status]);

  const handleStatusChange = (newStatus) => {
    setStatus(newStatus);
  };

  const handleOpenForm = () => {
    setIsFormOpen(true);
  };

  const handleCloseForm = () => {
    setIsFormOpen(false);
    fetchDelivery();
  };

  return (
    <div className="delivery-container">
      <h2>Delivery Filters</h2>

      <div className="filters-container">
        <div className="status-buttons">
          <button onClick={() => handleStatusChange("pending")}
            className={`btn-filter ${status === "pending" ? "active" : ""}`}>
            Pending
          </button>
          <button onClick={() => handleStatusChange("in process")}
            className={`btn-filter ${status === "in process" ? "active" : ""}`}>
            In Process
          </button>
          <button onClick={() => handleStatusChange("completed")}
            className={`btn-filter ${status === "completed" ? "active" : ""}`}>
            Completed
          </button>
          <button onClick={() => handleStatusChange("")}
            className={`btn-filter ${status === "" ? "active" : ""}`}>
            All
          </button>
        </div>

        <div className="date-filters-container">
            <div className="filter-item">
              <label htmlFor="startDate">Start Date:</label>
              <input
                id="startDate"
                type="date"
                value={startDate}
                onChange={(e) => setStartDate(e.target.value)}
              />
            </div>

            <div className="filter-item">
              <label htmlFor="endDate">End Date:</label>
              <input
                id="endDate"
                type="date"
                value={endDate}
                onChange={(e) => setEndDate(e.target.value)}
              />
            </div>

            <div className="filter-item">
              <button onClick={fetchDelivery} className="btn btn-apply-filters">Apply Filters</button>
            </div>
        </div>
        
        <div className="actions">
          <button onClick={handleOpenForm} className="btn-act">Add Delivery</button>
          <button onClick={assignCouriers} className="btn-act">Assign Couriers</button>
        </div>
      </div>

      <DeliveryContainer
        deliveries={delivery}
        isLoading={isLoading}
        error={error}
      />

      <DeliveryForm
        open={isFormOpen}
        onClose={handleCloseForm}
      />
    </div>
  );
};

export default Delivery;
