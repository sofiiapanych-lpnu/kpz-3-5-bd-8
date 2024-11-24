// DeliveryCard.jsx
import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { useParams } from 'react-router-dom';

const DeliveryCard = ({ delivery }) => {
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
                    
                </div>
                <Link className="nav-link" to={`/delivery/${delivery.deliveryId}`}>Show more</Link>

            </div>
        </div>
    );
};

export default DeliveryCard;
