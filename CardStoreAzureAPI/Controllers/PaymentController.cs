using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CardStoreAzureAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PaymentController : ApiController
    {

        [HttpGet]
        [Route("api/Payment/TestWebAPI")]
        public string TestWebAPI()
        {
            return "API WORKS";
        }


        [HttpGet]
        [Route("api/Payment/GetAllPayments")]
        public IEnumerable<PaymentDetail> GetAllPayments()
        {
            using (PaymentDetailsEntities entities = new PaymentDetailsEntities())
            {
                return entities.PaymentDetails.ToList();
            }
        }

        /// <summary>
        /// Returns Payment by Id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Payment/GetPersonById/{id}")]
        public HttpResponseMessage GetPersonById(int id)
        {
            if (id <= 0)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Id should start from 1");
            }
            using (PaymentDetailsEntities entities = new PaymentDetailsEntities())
            {
                PaymentDetail details = entities.PaymentDetails.Where(x => x.PMId == id).FirstOrDefault();
                if (details == null)
                {
                    Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Payment with Id specified not found !");
                }
                return Request.CreateResponse(details);
            }
        }

        [HttpPost]
        [Route("api/Payment/SavePayment")]
        public HttpResponseMessage SavePayment([FromBody] PaymentDetail emp)
        {
            if (string.IsNullOrEmpty(emp.CardNumber) || string.IsNullOrEmpty(emp.CardOwnerName) || string.IsNullOrEmpty(emp.CVV))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Mandatory fields cannot be empty!");
            }
            using (PaymentDetailsEntities entities = new PaymentDetailsEntities())
            {
                entities.PaymentDetails.Add(emp);
                entities.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Save Successful");
            }
        }

        [HttpPut]
        [Route("api/Payment/UpdatePaymentDetails")]
        public HttpResponseMessage UpdatePaymentDetails([FromBody] PaymentDetail emp)
        {
            if (string.IsNullOrEmpty(emp.CardOwnerName) || string.IsNullOrEmpty(emp.CardNumber))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Mandatory fields cannot be empty!");
            }
            using (PaymentDetailsEntities entities = new PaymentDetailsEntities())
            {
                var updatePaymentData = entities.PaymentDetails.FirstOrDefault(x => x.PMId == emp.PMId);
                updatePaymentData.CardNumber = emp.CardNumber;
                updatePaymentData.CardOwnerName = emp.CardOwnerName;
                entities.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Save Successful");
            }
        }
    }
}

