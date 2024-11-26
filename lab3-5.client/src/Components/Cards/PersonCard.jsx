import React from 'react';
import { Link } from 'react-router-dom';
import './DeliveryCard.css'

const PersonCard = ({ person }) => {
    return (
        <div className="card mb-4 delivery-card">
            <div className="card-body">
                <div className="highlight-id">
                    <h4>Person ID: {person.personId}</h4>
                </div>
                <div className="details">
                    <div className="detail-item">
                        <h5>Last Name: <span>{person.lastName}</span></h5>
                    </div>
                </div>
                <div className="text-end">
                    <Link className="btn btn-sm" to={`/person/${person.personId}`}>Show more</Link>
                </div>
            </div>
        </div>
    );
};

export default PersonCard;

