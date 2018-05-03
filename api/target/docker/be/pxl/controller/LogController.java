package be.pxl.controller;

import be.pxl.entity.logging.Log;
import be.pxl.service.ILogService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;
@CrossOrigin
@RestController
@RequestMapping(LogController.BASE_URL)
public class LogController {
    @Autowired
    private ILogService service;


    public static final String BASE_URL= "/api/logs";
    // Get log by ID
    @RequestMapping(value="/get/{id}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<Log> getLog(@PathVariable("id") int id) {
        HttpStatus status = HttpStatus.OK;

        Log log = service.find(id);
        if (log==null) {
            log = new Log();
        }

        return new ResponseEntity<>(log, status);
    }
    // Add log
    @RequestMapping(value ="/add", method = RequestMethod.POST)
    @ResponseStatus(value=HttpStatus.CREATED)
    public void addLog(@RequestBody Log log) {
        service.persist(log);
    }
    // Update log
    @RequestMapping(value ="/update", method = RequestMethod.PUT)
    public void updateLog(@RequestBody Log log) {
        service.update(log);
    }

    @RequestMapping(value ="/all", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<Log> getAllLogs() {
        return service.All();
    }
}
