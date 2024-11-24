import { useState, useEffect } from "react";
import axios from 'axios';
import DeliveryContainer from "../../Components/CardsContainers/DeliveryContainer";
import DeliveryForm from "../../Components/Forms/DeliveryForm";

const Delivery = () => {
  const [delivery, setDelivery] = useState([]);
  const [rows, setRows] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  const [status, setStatus] = useState("");
  const [startDate, setStartDate] = useState("");
  const [endDate, setEndDate] = useState("");

  const [isFormOpen, setIsFormOpen] = useState(false);

  // Функція для фільтрації і отримання даних
  const fetchDelivery = () => {
    setIsLoading(true);
    let queryParams = "";

    // Формуємо параметри запиту з фільтрів
    if (status) queryParams += `?status=${status}`;
    if (startDate) queryParams += `${queryParams ? "&" : "?"}startDate=${startDate}`;
    if (endDate) queryParams += `${queryParams ? "&" : "?"}endDate=${endDate}`;

    console.log("Fetching deliveries with query:", queryParams); // Додамо логування для перевірки запиту

    axios
      .get(`api/Delivery${queryParams}`)
      .then(response => {
        console.log(response.data)
        setDelivery(response.data);
        
        
        setIsLoading(false);
      })
      .catch(error => {
        setError('Error fetching deliveries.');
        setIsLoading(false);
      });
  };

  const assignCouriers = () => {
    axios
      .post("api/Delivery/assign-couriers")
      .then((response) => {
        console.log("Couriers assigned successfully:", response.data);
        fetchDelivery()
        alert(response.data.message);
      })
      .catch((error) => {
        console.error("Error assigning couriers:", error);
        alert("An error occurred while assigning couriers.");
      });
  };
  

  useEffect(() => {
    fetchDelivery();
  }, [status]);

  const handleStatusChange = (newStatus) => {
    setStatus(newStatus);
    console.log("Selected status:", newStatus);
  };

  const handleOpenForm = () => {
    setIsFormOpen(true);
  };

  const handleCloseForm = () => {
    setIsFormOpen(false);
    fetchDelivery();
  };



  return (
    <div className="container mt-4">
      <h2>Delivery Filters</h2>

      <div className="btn-group mb-4">
        <button onClick={() => handleStatusChange("pending")} className="btn btn-secondary">Pending</button>
        <button onClick={() => handleStatusChange("in process")} className="btn btn-secondary">In Process</button>
        <button onClick={() => handleStatusChange("completed")} className="btn btn-secondary">Completed</button>
        <button onClick={() => handleStatusChange("")} className="btn btn-secondary">All</button>
      </div>

      <div className="mb-4">
        <div className="form-group">
          <label htmlFor="startDate">Start Date:</label>
          <input
            id="startDate"
            type="date"
            className="form-control"
            value={startDate}
            onChange={(e) => setStartDate(e.target.value)}
          />
        </div>

        <div className="form-group">
          <label htmlFor="endDate">End Date:</label>
          <input
            id="endDate"
            type="date"
            className="form-control"
            value={endDate}
            onChange={(e) => setEndDate(e.target.value)}
          />
        </div>

        <button onClick={fetchDelivery} className="btn btn-primary">Apply Filters</button>
      </div>

      <button onClick={() => handleOpenForm()} className="btn btn-success mb-4">
        Add Delivery
      </button>
      <button onClick={assignCouriers} className="btn btn-info mb-4">
        Assign Couriers
      </button>
      

      <DeliveryContainer
        deliveries={delivery}
        isLoading={isLoading}
        error={error}
        // onEdit={(delivery) => handleOpenForm(delivery)}
      />

        <DeliveryForm
          //delivery={delivery}
          open={isFormOpen}
          onClose={handleCloseForm}
        />
    </div>
  );
};

export default Delivery;
