import { Link } from "react-router-dom";
import Table from "../../Components/Table/Table";
import { useEffect, useState } from "react";
import axios from 'axios';

const Person = ()=>{
  const [people, setPeople] = useState([]);
  const [columns, setColumns] = useState([]);
  const [rows, setRows] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);

  const fetchPeople = () =>{
    setIsLoading(true);
    axios.get('api/Person')
      .then(response => {
        setPeople(response.data);
        if (response.data.length > 0) {
          const keys = Object.keys(response.data[0]);
          setColumns(
            keys.map((key) => ({
              field: key,
              headerName: key
              .replace(/([a-z])([A-Z])/g, "$1 $2")
              .replace(/([A-Z])([A-Z][a-z])/g, "$1 $2")
              .replace(/^\w/, (c) => c.toUpperCase())
              .replace(/\s\w/g, (c) => c.toLowerCase()),
            }))
          );

          setRows(
            response.data.map((item) => ({
              id: item.personId,
              ...item,
            }))
          );

        }
      
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
  console.log(rows)

  return (
    <div>
      <Table columns={columns} initialRows={rows} entityType={"Person"}/>
    </div>
  );
}

export default Person;