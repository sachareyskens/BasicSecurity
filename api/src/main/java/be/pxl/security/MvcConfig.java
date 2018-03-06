package be.pxl.security;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.http.CacheControl;
import org.springframework.web.servlet.config.annotation.ResourceHandlerRegistry;
import org.springframework.web.servlet.config.annotation.ViewControllerRegistry;
import org.springframework.web.servlet.config.annotation.WebMvcConfigurerAdapter;
import org.springframework.web.servlet.resource.WebJarsResourceResolver;
import org.springframework.web.servlet.view.InternalResourceViewResolver;
import org.springframework.web.servlet.view.JstlView;

import java.util.concurrent.TimeUnit;

@Configuration
public class MvcConfig extends WebMvcConfigurerAdapter {
    @Override
    public void addViewControllers(ViewControllerRegistry view) {

        view.addViewController("/index").setViewName("index");
        view.addViewController("/").setViewName("index");
        view.addViewController("/error").setViewName("error");
        view.addViewController("/login").setViewName("login");
        view.addViewController("/about").setViewName("about");
        view.addViewController("/contact").setViewName("contact");
        view.addViewController("/template").setViewName("template");
        view.addViewController("/error").setViewName("error");
        view.addViewController("/apiindex").setViewName("apiindex");


    }
    @Override
    public void addResourceHandlers(ResourceHandlerRegistry registry) {
        registry.addResourceHandler("/static/**")
                .addResourceLocations("/resources/", "/META-INF/resources/webjars/")
                .setCacheControl(
                        CacheControl.maxAge(30L, TimeUnit.DAYS).cachePublic())
                .resourceChain(true)
                .addResolver(new WebJarsResourceResolver());
        registry.addResourceHandler("/META-INF/resources/webjars/");
    }



    @Bean
    public InternalResourceViewResolver viewResolver(){
        InternalResourceViewResolver viewResolver = new InternalResourceViewResolver();
        viewResolver.setViewClass(JstlView.class);
        viewResolver.setPrefix("/templates/");
        viewResolver.setSuffix(".html");
        return viewResolver;
    }



}
