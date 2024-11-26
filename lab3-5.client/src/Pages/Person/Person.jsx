import { Link } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from 'axios';
import PersonForm from '../../Components/Forms/PersonForm'
import PersonContainer from '../../Components/CardsContainers/PersonContainer'

const Person = ()=>{
  const [people, setPeople] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [isFormOpen, setIsFormOpen] = useState(false);

  const fetchPeople = () =>{
    setIsLoading(true);
    axios.get('api/Person')
      .then(response => {
        setPeople(response.data);
        setIsLoading(false);
      })
      .catch(err => {
        setError('Error fetching people.');
        setIsLoading(false);
      });
  }
  console.log(people)

  useEffect(() => {
    fetchPeople();
  }, []);

  const handleOpenForm = () => {
    setIsFormOpen(true);
  };

  const handleCloseForm = () => {
    setIsFormOpen(false);
    fetchPeople();
  };

  return (
    <div className="delivery-container">
      <h2>Person</h2>

      <button onClick={handleOpenForm} className="btn-act">Add Person</button>

      <PersonContainer
        people={people}
        isLoading={isLoading}
        error={error}
      />

      <PersonForm
        open={isFormOpen}
        onClose={handleCloseForm}
      />
    </div>
  );
}

export default Person;