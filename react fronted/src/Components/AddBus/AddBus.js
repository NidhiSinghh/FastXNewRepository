
import { useState } from "react";
import './AddBus.css'
function AddBus(){

    var [busName,setBusName]=useState("");
    var [busType,setBusType]=useState("");
    var [totalSeats,setTotalSeats]=useState("");
    var [busOperatorId,setBusOperatorId]=useState("");

    var bus={};

    var addBus=()=>{
        bus.busName=busName;
        bus.busType=busType;
        bus.totalSeats=totalSeats;
        bus.busOperatorId=busOperatorId;
        //console.log(bus);
        var requestOptions = {
            method :'POST',
            headers: {'Content-Type':'application/json'},
            body : JSON.stringify(bus)
        }
        console.log(requestOptions);
        fetch("http://localhost:5263/api/Bus/AddBusByBusOperator",requestOptions)
        .then(res=>res.json())
        .then(res=>console.log(res))
        .catch(err=>console.log(err));

        
    }
   // console.log(addBus);
    return(
        <div>
           

    <div className="container mt-5 black">
      <h2>Bus Information Form</h2>
      {/* <form > */}
      {/* onSubmit={handleSubmit} */}
        <div className="mb-3">
          <label htmlFor="busName" className="form-label">
            Bus Name:
          </label>
          <input
            type="text"
            className="form-control"
            id="busName"
            name="busName"
            value={busName}
            onChange={(e)=>setBusName(e.target.value)}
            // required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="bustype" className="form-label">
            Bus Type:
          </label>
          <input
            type="text"
            className="form-control"
            id="bustype"
            name="bustype"
            value={busType}
           onChange={(e)=>setBusType(e.target.value)}
            //required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="totalSeats" className="form-label">
            Total Number of Seats:
          </label>
          <input
            type="number"
            className="form-control"
            id="totalSeats"
            name="totalSeats"
           value={totalSeats}
            onChange={(e)=>setTotalSeats(e.target.value)}
           // required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="operatorId" className="form-label">
            Bus Operator ID:
          </label>
          <input
            type="text"
            className="form-control"
            id="operatorId"
            name="operatorId"
           value={busOperatorId}
            onChange={(e)=>setBusOperatorId(e.target.value)}
           // required
          />
        </div>
        <button onClick={addBus} className="btn btn-primary">
          Submit
        </button>
      {/* </form> */}
    </div>





        </div>
    )
}
export default AddBus;