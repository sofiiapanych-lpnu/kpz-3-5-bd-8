import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import DeliveryForm from '../Forms/DeliveryForm';

const DeliveryFullCard = () => {
    const navigate = useNavigate();
    const [delivery, setDelivery] = useState({});
    const [isOpen, setOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const params = useParams();

    const handleOpenModal = () => {
        startTransaction();
        setOpen(true);
    };

    const handleCloseModal = () => {
        getInfo();
        setOpen(false);
    };

    const startTransaction = () => {
        axios.post(`/api/Delivery/start-transaction`)
            .then(response => alert(response.data))
            .catch(() => {
                setError('Error starting transaction.');
                setIsLoading(false);
            });
    };

    const getInfo = () => {
        axios.get(`/api/Delivery/${params.deliveryId}`)
            .then(response => {
                setDelivery(response.data);
                setIsLoading(false);
            })
            .catch(() => {
                setError('Error fetching delivery data.');
                setIsLoading(false);
            });
    };

    useEffect(() => {
        getInfo();
    }, [params.deliveryId]);

    const handleDelete = () => {
        if (window.confirm('Are you sure you want to delete this delivery?')) {
            axios.delete(`/api/Delivery/${params.deliveryId}`)
                .then(() => navigate('/delivery'))
                .catch(err => {
                    alert(`Error deleting delivery: ${err}`);
                    navigate('/delivery');
                });
        }
    };

    return (
        <div className="delivery-card">
            <div className="section section-highlight">
                <h4>Delivery ID: {delivery.deliveryId}</h4>
                <p><strong>Status:</strong> {delivery.status}</p>
            </div>
            <div className='d-flex justify-content-between align-items-start'>
                <div className="section section-details">
                    <h5>Details</h5>
                    <p><strong>Order ID:</strong> {delivery.orderId}</p>
                    <p><strong>Courier ID:</strong> {delivery.courierId}</p>
                    <p><strong>Warehouse ID:</strong> {delivery.warehouseId}</p>
                    <p><strong>Address ID:</strong> {delivery.addressId}</p>
                </div>

                <div className="section section-timing">
                    <h5>Timing</h5>
                    <p><strong>Start Time:</strong> {new Date(delivery.startTime).toLocaleString()}</p>
                    <p><strong>End Time:</strong> {new Date(delivery.endTime).toLocaleString()}</p>
                    <p><strong>Desired Duration:</strong> {delivery.desiredDuration}</p>
                    <p><strong>Actual Duration:</strong> {delivery.actualDuration}</p>
                </div>
            </div>
            

            <div className="section section-actions">
                <button className="btn btn-danger" onClick={handleDelete}>
                    <i className="fas fa-trash-alt"></i> Delete
                </button>
                <button className="btn btn-secondary mx-2" onClick={handleOpenModal}>
                    Edit
                </button>
            </div>

            <DeliveryForm
                delivery={delivery}
                open={isOpen}
                onClose={handleCloseModal}
            />
        </div>
    );
};

export default DeliveryFullCard;
