// DeliveryCard.jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import DeliveryForm from '../Forms/DeliveryForm'

const DeliveryFullCard = ({ }) => {
  const navigate = useNavigate();
  console.log('params, ppppppppp')
  const [delivery, setDelivery] = useState({});

    const [isOpen, setOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);

    const params = useParams();
console.log(params, 'ppppppppp')

    const handleOpenModal = () => {
        startTransaction();
        setOpen(true);
    }

    const handleCloseModal = () => {
        getInfo();
        setOpen(false);
    }

    const startTransaction = () => {
        axios.post(`/api/Delivery/start-transaction`)
            .then(response => {
                console.log(response.data);
                alert(response.data);
            })
            .catch(err => {
                setError('Error starting transaction.');
                setIsLoading(false);
            });
    }

    const getInfo = () => {
      axios.get(`/api/Delivery/${params.deliveryId}`)
          .then(response => {
              console.log(response.data);
              setDelivery(response.data);

              setIsLoading(false);
          })
          .catch(err => {
              setError('Error fetching people.');
              setIsLoading(false);
          });
  }

  useEffect(()=>{
    getInfo();
  }, [params.deliveryId])

    const handleDelete = () => {
        if (window.confirm('Are you sure you want to delete this delivery?')) {
            axios.delete(`/api/Delivery/${params.deliveryId}`)
                .then(() => {
                    navigate('/delivery');
                })
                .catch(err => {
                    alert(`Error deleting delivery. ${err}`);
                    navigate('/delivery');
                });
        }
    };
    return (
        <div className="card mb-3">
            <div className="card-body">
                <div className="d-flex justify-content-between">
                    <div>
                        <h4>Delivery ID: {delivery.deliveryId}</h4>
                        <h5>Status: {delivery.status}</h5>
                        <p>Order ID: {delivery.orderId}</p>
                        <p>Courier ID: {delivery.courierId}</p>
                    </div>
                    <div>
                        <h6 className="text-muted">
                            <strong>Warehouse ID:</strong> {delivery.warehouseId}
                        </h6>
                        <h6 className="text-muted">
                            <strong>Address ID:</strong> {delivery.addressId}
                        </h6>
                        {/* <Link className="btn btn-primary mt-2" to={`/deliveries/${delivery.deliveryId}`}>
                            Show Details
                        </Link> */}
                    </div>
                </div>
                <div className="mt-3">
                    <p>
                        <strong>Start Time:</strong> {new Date(delivery.startTime).toLocaleString()}
                    </p>
                    <p>
                        <strong>End Time:</strong> {new Date(delivery.endTime).toLocaleString()}
                    </p>
                    <p>
                        <strong>Desired Duration:</strong> {delivery.desiredDuration}
                    </p>
                    <p>
                        <strong>Actual Duration:</strong> {delivery.actualDuration}
                    </p>
                </div>
                <button className="btn btn-danger btn-sm" onClick={handleDelete}>
                    <i className="fas fa-trash-alt"></i> Delete
                </button>
                <button className="btn btn-secondary mx-2" onClick={() => handleOpenModal()}>
                    Edit
                </button>
                <DeliveryForm
                  delivery={delivery}
                  open={isOpen}
                  onClose={handleCloseModal}
                />
            </div>
        </div>
    );
};

export default DeliveryFullCard;
