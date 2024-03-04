 [HttpGet("GetBusForBusOperator")]
        public async Task<ActionResult<Bus>> GetBusesByOperatorId([FromQuery]int busOperatorId)
        {
            try
            {
                var buses = await _busOperatorService.GetAllBuses(busOperatorId);

                return Ok(buses);
            }
            catch (BusOperatorNotFoundException ex)
            {
                _logger.LogWarning(ex.Message);
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving buses for bus operator ID: {BusOperatorId}", busOperatorId);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }








        //---------------------------
        // POST: api/Bus/AddAmenity 
        //this adds Amenity when BusId and Amenity name is given

        //[Authorize(Roles = "busoperator")] //this will add into busamenities table
        //[HttpPost("AddAmenityForBusByBusOperator")]
        //public async Task<IActionResult> AddAmenity([FromBody]AddAmenityDTO addAmenity)
        //{
        //    try
        //    {
        //        await _amenityService.AddAmenityToBus(addAmenity.BusId, addAmenity.AmenityName);
        //        return Ok("Amenity added successfully");
        //    }
        //    catch (BusNotFoundException ex)
        //    {
        //        _logger.LogError(ex, "Bus not found");
        //        return NotFound(ex.Message);
        //    }
        //    catch (AmenityAlreadyExistsException ex)
        //    {
        //        _logger.LogError(ex, "Amenity already exists for this bus");
        //        return Conflict(ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "An error occurred while adding amenity to bus");
        //        return StatusCode(500, "Internal server error");
        //    }
        //}


        [HttpPost("AddAmenitiesToBus")]
        public async Task<IActionResult> AddAmenitiesForBus([FromBody] AddAmenityDTO request)
        {
            try
            {
                await _amenityService.AddAmenitiesToBus(request.BusId, request.AmenityNames);
                var response = new
                {
                    Message = "Amenities added successfully for the bus."
                };

                return Ok(response);
            }
            catch (BusNotFoundException ex)
            {
                var errorResponse = new
                {
                    Message = $"Bus not found: {ex.Message}"
                };

                return NotFound(errorResponse);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    Message = $"An error occurred: {ex.Message}"
                };

                return StatusCode(500, errorResponse);
            }
        }


        //[Authorize(Roles = "busoperator")] //this will delete from busamenities table
        
        [HttpDelete("DeleteAmenityForBusByBusOperator")]
        public async Task<IActionResult> DeleteAmenity(int busId, string amenityName)
        {
            try
            {
                await _amenityService.DeleteAmenityFromBus(busId, amenityName);
                return Ok("Amenity deleted successfully");
            }
            catch (BusNotFoundException ex)
            {
                _logger.LogError(ex, "Bus not found");
                return NotFound(ex.Message);
            }
            catch (AmenitiesNotFoundException ex)
            {
                _logger.LogError(ex, "Amenity not found");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting amenity from bus");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("AcceptRefund")]
        public async Task<IActionResult>AcceptRefund(int bookingId, int userId)
        {
            await _busOperatorService.AcceptRefund(bookingId, userId);
            return Ok();
        }
    }
}