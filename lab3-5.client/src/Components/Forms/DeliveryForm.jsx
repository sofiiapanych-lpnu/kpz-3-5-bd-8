import { useState, useEffect } from 'react';
import Modal from './Modal';
import axios from 'axios';
import './Form.css'

export default function DeliveryForm({ delivery, open, onClose }) {
  const [formData, setFormData] = useState({
    OrderId: '',
    CourierId: '',
    StartTime: '',
    EndTime: '',
    DesiredDuration: '',
    ActualDuration: '',
    WarehouseId: '',
    AddressId: '',
    Status: '',
  });

  useEffect(() => {
    if (delivery) {
        console.log(delivery)
      setFormData({
        OrderId: delivery.orderId || '',
        CourierId: delivery.courierId || '',
        StartTime: delivery.startTime || '',
        EndTime: delivery.endTime || '',
        DesiredDuration: delivery.desiredDuration || '',
        ActualDuration: delivery.actualDuration || '',
        WarehouseId: delivery.warehouseId || '',
        AddressId: delivery.addressId || '',
        Status: delivery.status || '',
      });
    } else {
      setFormData({
        OrderId: '',
        CourierId: '',
        StartTime: '',
        EndTime: '',
        DesiredDuration: '',
        ActualDuration: '',
        WarehouseId: '',
        AddressId: '',
        Status: '',
      });
    }
  }, [delivery]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();

    if (delivery) {
      console.log(formData)
      axios.put(`/api/Delivery/${delivery.deliveryId}`, formData)
        .then(response => {
          alert(response.data)
          console.log("Delivery successfully updated:", response);
          onClose();
        })
        .catch(error => {
            if (error.response && error.response.status === 400) {
                const errorMessages = error.response.data.errors;

                let fullErrorMessage = '';
                for (const field in errorMessages) {
                    if (errorMessages.hasOwnProperty(field)) {
                        fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                    }
                }
                alert(fullErrorMessage);
            }
            onClose();
            console.error('', error);
        });
    } else {
        axios.post('/api/Delivery', formData)
            .then(response => {
                console.log('', response.data);
                onClose();
            })
            .catch(error => {
                if (error.response && error.response.status === 400) {
                    const errorMessages = error.response.data.errors;

                    let fullErrorMessage = '';
                    for (const field in errorMessages) {
                        if (errorMessages.hasOwnProperty(field)) {
                            fullErrorMessage += `${field}: ${errorMessages[field].join(', ')}\n`;
                        }
                    }

                    alert(fullErrorMessage);
                }
                if (error.response && error.response.status === 401) {
                    alert(error.response.data.message + error.response.data.details);
                }
                onClose();
                console.error('', error);
            });
    }
  };


  return (
    <Modal open={open} onClick={onClose}>
        <form className="delivery-form" onSubmit={handleSubmit}>
            <h5 className="form-title">{delivery ? 'Edit Delivery Form' : 'Create Delivery Form'}</h5>

            <div className="form-section">
                <label htmlFor="OrderId" className="form-label">Order ID</label>
                <input
                    type="text"
                    className="form-input"
                    id="OrderId"
                    name="OrderId"
                    value={formData.OrderId}
                    onChange={handleChange}
                    maxLength="50"
                    required
                />
            </div>

            <div className="form-section">
                <label htmlFor="CourierId" className="form-label">Courier ID</label>
                <input
                    type="text"
                    className="form-input"
                    id="CourierId"
                    name="CourierId"
                    value={formData.CourierId}
                    onChange={handleChange}
                    maxLength="50"
                />
            </div>

            <div className="form-section">
                <label htmlFor="StartTime" className="form-label">Start Time</label>
                <input
                    type="datetime-local"
                    className="form-input"
                    id="StartTime"
                    name="StartTime"
                    value={formData.StartTime.replace(/\.\d+$/, "")}
                    onChange={handleChange}
                    step="1"
                />
            </div>

            <div className="form-section">
                <label htmlFor="EndTime" className="form-label">End Time</label>
                <input
                    type="datetime-local"
                    className="form-input"
                    id="EndTime"
                    name="EndTime"
                    value={formData.EndTime.replace(/\.\d+$/, "")}
                    onChange={handleChange}
                    step="1"
                />
            </div>

            <div className="form-section">
                <label htmlFor="DesiredDuration" className="form-label">Desired Duration</label>
                <input
                    type="time"
                    className="form-input"
                    id="DesiredDuration"
                    name="DesiredDuration"
                    value={formData.DesiredDuration}
                    onChange={handleChange}
                    required
                    step="1"
                />
            </div>

            <div className="form-section">
                <label htmlFor="ActualDuration" className="form-label">Actual Duration</label>
                <input
                    type="time"
                    className="form-input"
                    id="ActualDuration"
                    name="ActualDuration"
                    value={formData.ActualDuration}
                    onChange={handleChange}
                    step="1"
                />
            </div>

            <div className="form-section">
                <label htmlFor="WarehouseId" className="form-label">Warehouse ID</label>
                <input
                    type="text"
                    className="form-input"
                    id="WarehouseId"
                    name="WarehouseId"
                    value={formData.WarehouseId}
                    onChange={handleChange}
                    required
                />
            </div>

            <div className="form-section">
                <label htmlFor="AddressId" className="form-label">Address ID</label>
                <input
                    type="text"
                    className="form-input"
                    id="AddressId"
                    name="AddressId"
                    value={formData.AddressId}
                    onChange={handleChange}
                    required
                />
            </div>

            <div className="form-section">
                <label htmlFor="Status" className="form-label">Status</label>
                <select
                    className="form-input"
                    id="Status"
                    name="Status"
                    value={formData.Status}
                    onChange={handleChange}
                    required
                >
                    <option value="" disabled>Select status</option>
                    <option value="pending">Pending</option>
                    <option value="in process">In Process</option>
                    <option value="completed">Completed</option>
                </select>
            </div>


            <div className="form-actions">
                <button type="button" className="btn-secondary" onClick={onClose}>Close</button>
                <button type="submit" className="btn-primary">Save</button>
            </div>
        </form>
    </Modal>

  );
}
