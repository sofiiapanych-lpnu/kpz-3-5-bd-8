import {React} from 'react';
import PersonCard from '../Cards/PersonCard';

const PersonContainer = ({ people, isLoading, error }) => {
  if (isLoading) {
    return <p>Loading...</p>;
  }

  if (error) {
    return <p>{error}</p>;
  }

  return (
    <div className="row">
      {people.map(person => (
        <div className="col-12" key={person.personId}>
          <PersonCard person={person} />
        </div>
      ))}
    </div>
  );
};

export default PersonContainer;
