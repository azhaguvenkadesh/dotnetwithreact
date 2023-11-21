import React, { useState } from 'react';
import "./LoanApplicationForm.css";
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

function LoanApplicationForm() {
    const navigate = useNavigate();

  const [formData, setFormData] = useState({
    requestedAmount: '',
    submissionDate: '',
    employmentStatus: '',
    income: '',
    creditScore: '',
  });

  const [errors, setErrors] = useState({});

  async function handleSubmit() {
    const fieldErrors = {};


    if (formData.requestedAmount === '') {
      fieldErrors.requestedAmount = 'Requested Amount is required';
    }

    if (formData.submissionDate === '') {
      fieldErrors.submissionDate = 'Submission Date is required';
    }

    if (formData.employmentStatus === '') {
      fieldErrors.employmentStatus = 'Employment Status is required';
    }

    if (formData.income === '') {
      fieldErrors.income = 'Income is required';
    }

    if (formData.creditScore === '') {
      fieldErrors.creditScore = 'Credit Score is required';
    }


    setErrors(fieldErrors);

    // Check if there are any errors
    const hasErrors = Object.values(fieldErrors).some((error) => error !== '');

    if (hasErrors) {
      // Handle the form errors, e.g., display a message or style the input fields
    } else {
      // Proceed with form submission

let requestObject ={
    "userId":localStorage.getItem("userId"),
    "userName":localStorage.getItem("userName"),
    "loanType":localStorage.getItem("loanType"),
    "requestedAmount":formData.requestedAmount,
    "submissionDate":formData.submissionDate,
    "employmentStatus":formData.employmentStatus,
    "income":formData.income,
    "creditScore":formData.creditScore,
    "loanStatus":0
}
console.log("requestObject",requestObject)
try{
  const response = await axios.post("https://8080-dedfbebad306621518acccacedaone.premiumproject.examly.io/api/LoanApplication/AddLoanApplication",requestObject);
    console.log("response in application",response)
  if (response.status == 200) {
navigate("/appliedloan")
  }
//axios call
console.log('Form data:', requestObject);

}
catch{
  alert("Something Went wrong")
}
      // Add logic to submit the data (e.g., send it to a server)
    }
  }

  return (
    <div>
    <button onClick={()=>{
        navigate(-1)
    }}> Back</button>
      <h2>Loan Application Form</h2>
      <div id="container">

        <div>
          <label for="RequestedAmount" >Requested Amount:</label>
          <input
          id="RequestedAmount"
            type="text"
            name="requestedAmount"
            value={formData.requestedAmount}
            onChange={(e) => setFormData({ ...formData, requestedAmount: e.target.value })}
          />
          {errors.requestedAmount && <div className="error">{errors.requestedAmount}</div>}
        </div>

        <div>
          <label for="submissionDate">Submission Date:</label>
          <input
          id="submissionDate"
            type="date"
            name="submissionDate"
            value={formData.submissionDate}
            onChange={(e) => setFormData({ ...formData, submissionDate: e.target.value })}
          />
          {errors.submissionDate && <div className="error">{errors.submissionDate}</div>}
        </div>

        <div>
          <label for="employmentStatus">Employment Status:</label>
          <input
          id="employmentStatus"
            type="text"
            name="employmentStatus"
            value={formData.employmentStatus}
            onChange={(e) => setFormData({ ...formData, employmentStatus: e.target.value })}
          />
          {errors.employmentStatus && <div className="error">{errors.employmentStatus}</div>}
        </div>

        <div>
          <label for="income">Income:</label>
          <input
          id="income"
            type="text"
            name="income"
            value={formData.income}
            onChange={(e) => setFormData({ ...formData, income: e.target.value })}
          />
          {errors.income && <div className="error">{errors.income}</div>}
        </div>

        <div>
          <label for="creditScore">Credit Score:</label>
          <input

          id="creditScore"
            type="text"
            name="creditScore"
            value={formData.creditScore}
            onChange={(e) => setFormData({ ...formData, creditScore: e.target.value })}
          />
          {errors.creditScore && <div className="error">{errors.creditScore}</div>}
        </div>



        <button type="button" className="submit-button" onClick={handleSubmit}>
          Submit
        </button>
      </div>
    </div>
  );
}

export default LoanApplicationForm;
