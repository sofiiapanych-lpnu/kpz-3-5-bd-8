import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useParams, useNavigate } from 'react-router-dom';
import PersonForm from '../Forms/PersonForm';

const PersonFullCard = () => {
    const navigate = useNavigate();
    const [person, setPerson] = useState({});
    const [isOpen, setOpen] = useState(false);
    const [isLoading, setIsLoading] = useState(true);
    const [error, setError] = useState(null);
    const params = useParams();

    const handleOpenModal = () => {
      setOpen(true);
    };

    const handleCloseModal = () => {
      getInfo();
      setOpen(false);
    };

    const getInfo = () => {
      axios.get(`/api/Person/${params.personId}`)
          .then(response => {
            setPerson(response.data);
            setIsLoading(false);
          })
          .catch(() => {
            setError('Error fetching delivery data.');
            setIsLoading(false);
          });
    };

    useEffect(() => {
        getInfo();
    }, [params.personId]);

    const handleDelete = () => {
        if (window.confirm('Are you sure you want to delete this person?')) {
            axios.delete(`/api/Person/${params.personId}`)
                .then(() => navigate('/person'))
                .catch(err => {
                    alert(`Error deleting person: ${err}`);
                    navigate('/person');
                });
        }
    };

    return (
        <div className="delivery-card">
            <div className="section section-highlight">
                <h4>Person ID: {person.personId}</h4>
            </div>
            <div className='d-flex justify-content-between align-items-start'>
                <div className="section section-details">
                  <p><strong>First Name:</strong> {person.firstName}</p>
                  <p><strong>Last Name:</strong> {person.lastName}</p>
                </div>

                <div className="section section-timing">
                  <p><strong>Phone Number:</strong> {person.phoneNumber}</p>
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

            <PersonForm
                person={person}
                open={isOpen}
                onClose={handleCloseModal}
            />
        </div>
    );
};

export default PersonFullCard;
