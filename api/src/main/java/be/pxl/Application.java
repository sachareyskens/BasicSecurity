package be.pxl;

import org.apache.activemq.command.ActiveMQQueue;
import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.builder.SpringApplicationBuilder;
import org.springframework.boot.web.support.SpringBootServletInitializer;
import org.springframework.context.annotation.Bean;
import org.springframework.jms.annotation.EnableJms;
import org.springframework.transaction.annotation.EnableTransactionManagement;

import javax.jms.Queue;

@SpringBootApplication
@EnableTransactionManagement
@EnableJms
public class Application extends SpringBootServletInitializer {
    @Bean(name = "JMSLoggerQueue")
    public Queue jmsLoggerQue() {
        return new ActiveMQQueue("LoggerQueue");
    }
    public static void main(String[] args) {
        // Run the application
        SpringApplication.run(Application.class, args);
    }

    @Override
    protected SpringApplicationBuilder configure(SpringApplicationBuilder builder) {
        return builder.sources(Application.class);
    }
}
