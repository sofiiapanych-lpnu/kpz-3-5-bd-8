import React from 'react';
import { Link } from 'react-router-dom';
import './DeliveryCard.css'

const DeliveryCard = ({ delivery }) => {
    return (
        <div className="card mb-4 delivery-card">
            <div className="card-body">
                <div className="highlight-id">
                    <h4>Delivery ID: {delivery.deliveryId}</h4>
                </div>
                <div className="details">
                    <div className="detail-item">
                        <h5>Status: <span>{delivery.status}</span></h5>
                    </div>
                    <div className="detail-item">
                        <p>Order ID: <span>{delivery.orderId}</span></p>
                    </div>
                    <div className="detail-item">
                        <p>Courier ID: <span>{delivery.courierId}</span></p>
                    </div>
                </div>
                <div className="text-end">
                    <Link className="btn  btn-sm" to={`/delivery/${delivery.deliveryId}`}>Show more</Link>
                </div>
            </div>
        </div>
    );
};

export default DeliveryCard;

