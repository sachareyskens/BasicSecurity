package be.pxl.aop;

import be.pxl.entity.Message;
import be.pxl.entity.logging.JMSMessageLogger;
import org.aspectj.lang.annotation.After;
import org.aspectj.lang.annotation.Aspect;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

@Aspect
@Component
public class PersistSuccesAspect {
    @Autowired
    private JMSMessageLogger logger;


    @After("execution(* *.decryptMessage(..)) && args(message, username)")
    public void decryptMessage(Message message, String username) {
        logger.log("Decrypted Message: " + message.getId() + " || Reciever: " + username);
    }

    @After("execution(* *.encryptMessage(..)) && args(message)")
    public void encryptMessage(Message message) {
        logger.log("Encrypted Message : " + message.getId() + " || Sender: " + message.getSender());
    }



}
