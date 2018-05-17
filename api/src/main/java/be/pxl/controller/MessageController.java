package be.pxl.controller;

import be.pxl.crypter.Crypter;
import be.pxl.entity.Message;
import be.pxl.entity.StatsSource;
import be.pxl.service.IMessageService;
import be.pxl.service.IUserService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.HttpStatus;
import org.springframework.http.MediaType;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.ArrayList;
import java.util.List;

@RestController
@CrossOrigin
@RequestMapping(MessageController.BASE_URL)
public class MessageController {
    @Autowired
    private IMessageService service;
    @Autowired
    private IUserService userService;
    @Autowired
    Crypter crypter;

    public static final String BASE_URL = "api/messages";

    // Gets a single message.
    @RequestMapping(value= "/get/{id}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<Message> getMessage(@PathVariable("id") int key) {
        HttpStatus status = HttpStatus.OK;
        Message message = service.find(key);

        return new ResponseEntity<>(message, status);
    }
    // Gets all messages.
    @RequestMapping(value= "/all", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<Message> AllMessages() {
        return service.All();
    }

    // Adds a message.
    @RequestMapping(value="/add", method = RequestMethod.POST, consumes = MediaType.APPLICATION_JSON_VALUE)
    public void addMessage(@RequestBody Message message) {


        service.persist(crypter.encryptMessage(message));

    }
    // Deletes a message
    @RequestMapping(value = "/delete/{id}", method = RequestMethod.DELETE)
    public void deleteMessage(@PathVariable("id") int id) {
        service.delete(id);
    }
    // Decrypt a message with the crypter class.
    @RequestMapping(value= "/decrypt/{id}", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<Message> decrypt(@PathVariable("id") int key, @RequestParam("loggedIn") String username) {
        HttpStatus status = HttpStatus.OK;
        try {
            Message message = service.find(key);
            Message messagee = crypter.decryptMessage(message, username);
            byte[] hidden = new byte[1];
            messagee.setEncryptedSymm(hidden);
            return new ResponseEntity<>(messagee, status);
        } catch (Exception e) {
            return new ResponseEntity<>(new Message(), HttpStatus.NOT_FOUND);
        }

    }
    // Show all messages from a certain user to a certain user.
    @RequestMapping(value= "/showall", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<Message> showAllByUsername(@RequestParam("username") String username) {
        return service.findByUsername(username);
    }
    // Decrypt all messages from this sender to this reciever.
    @RequestMapping(value= "/decrypt/all", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public List<Message> decryptAll(@RequestParam("sender") String sender, @RequestParam("reciever") String reciever) {

        try {
            List<Message> l, k = new ArrayList<>();
            l = service.findBySenderAndReciever(sender, reciever);

            for (Message m : l
                 ) {
                k.add(crypter.decryptMessage(m, reciever));

            }

            return k;
        } catch (Exception e) {
            return new ArrayList<>();
        }

    }
    // Count total amount of messages and users.
    @RequestMapping(value="/countAll", method = RequestMethod.GET, produces = "application/json;charset:utf-8")
    public ResponseEntity<StatsSource> getCounts() {
        HttpStatus status = HttpStatus.OK;
        StatsSource statsSource = new StatsSource();
        statsSource.setTotalUsers("Total users : " + userService.countAll());
        statsSource.setTotalMessages("Total messages : " + service.countAll());
        return new ResponseEntity<>(statsSource, status);
    }


}
